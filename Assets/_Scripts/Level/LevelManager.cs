using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Level Boundaries")]
    [Tooltip("The furthest point an Object spawns or despawns.")]
    public float maxBoundary = 30.0f; // the Z position objects spawn/despawn from the level

    [Tooltip("The closest point an Object spawns or despawns. Set to a negative value to have Objects go past the player before despawning.")]
    public float minBoundary = -10.0f; // the z position object despawn

    [Tooltip("How much can an object deviate from the centre when spawned (higher=wider)")]
    public float XPosSpawnVariance = 5.0f;

    [Tooltip("How fast objects move towards the Player by default in a Level.")]
    public float worldMoveSpeed = 10.0f;

    [Tooltip("How quickly do new Encounters occur during the level (in seconds).")]
    public float encounterRate = 5.0f;

    [Header("Level Encounters")]
    [Tooltip("Lists what kind of Encounters can appear in this level randomly.")]
    public List<LevelEncounterScriptableObject> encounterList;

    [Tooltip("Lists what kind of Obstacles can spawn between Encounters in this level.")]
    public List<ObstacleScriptableObject> obstaclesList;
    
    //public List<JobEncounterScriptableObject> jobEncounterList;
    //public LevelEncounterScriptableObject endLevelEncounter;

    private int obstacleSpawnCounter; // how many encounters have occured this level
    private bool isDoingEncounter = false; // is the level currently doing an encounter?

    [HideInInspector]
    public static LevelManager LevelInstance; // Static Singleton referenced by other objects

    [HideInInspector]
    public GameObject playerRef;

    private IEnumerator SpawnRandomObstacle()
    {
        if (isDoingEncounter == false)
        {            
            WorldObstacle obstacle = ObjectPool.SharedInstance.GetWorldObstacleObject();
            
            if (obstacle != null)
            {
                float randX = Random.Range(-XPosSpawnVariance, XPosSpawnVariance);
                obstacle.transform.position = new Vector3(randX, 0.0f, maxBoundary); // 1.0f * Random.Range(0, 3.0f) for Y variance
                obstacle.SetMoveSpeed(worldMoveSpeed);
                obstacle.SetDeactivatePoint(minBoundary);

                int randI = Random.Range(0, obstaclesList.Count);
                obstacle.SetupObstacle(obstaclesList[randI]);

                obstacle.gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnRandomObstacle());
    }

    private IEnumerator DoEncounter(LevelEncounterScriptableObject levelEncounter)
    {
        switch(levelEncounter.encounterType)
        {
            case EncounterType.Obstacles:
                CreateWorldObstacle(levelEncounter.objectToSpawn);
                break;
            case EncounterType.Enemies:
                CreateEnemyObstacle(levelEncounter.objectToSpawn);
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(levelEncounter.spawnRate);
        if (obstacleSpawnCounter > 0)
        {
            obstacleSpawnCounter--;
            StartCoroutine(DoEncounter(levelEncounter));
        }
        else
            isDoingEncounter = false;
    }

    private IEnumerator ChooseNewEncounter()
    {
        if (isDoingEncounter == false) // start a new encounter
        {
            //print("Selecting a new Encounter...");
            int randI = Random.Range(0, encounterList.Count);
            LevelEncounterScriptableObject selectedEncounter = encounterList[randI];

            obstacleSpawnCounter = selectedEncounter.amountToSpawn;
            StartCoroutine(DoEncounter(selectedEncounter));
        }
        
        yield return new WaitForSeconds(encounterRate);
        StartCoroutine(ChooseNewEncounter());
    }

    private void Awake()
    {
        LevelInstance = this;
        playerRef = GameObject.FindGameObjectWithTag("Player"); // store player reference with level
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRandomObstacle());
        StartCoroutine(ChooseNewEncounter());

    }

    private void CreateWorldObstacle(ObstacleScriptableObject obstacleData)
    {
        WorldObstacle obstacle = ObjectPool.SharedInstance.GetWorldObstacleObject();

        if (obstacle != null)
        {
            float randX = Random.Range(-XPosSpawnVariance, XPosSpawnVariance);
            obstacle.transform.position = new Vector3(randX, 0.0f, maxBoundary);
            obstacle.SetMoveSpeed(worldMoveSpeed);
            obstacle.SetDeactivatePoint(minBoundary);           
            obstacle.SetupObstacle(obstacleData);

            obstacle.gameObject.SetActive(true);
        }
    }

    private void CreateEnemyObstacle(ObstacleScriptableObject enemyData)
    {
        EnemyObstacle enemy = ObjectPool.SharedInstance.GetEnemyObstacleObject();
        if (enemy == null)
            return;

        float randX = Random.Range(-XPosSpawnVariance, XPosSpawnVariance);
        float spawnY = 0.0f;

        if(enemyData.isFloating)
            spawnY = 2.0f;
        
        enemy.transform.position = new Vector3(randX, spawnY, maxBoundary * 2);

        enemy.SetMoveSpeed(worldMoveSpeed);
        enemy.SetDeactivatePoint(minBoundary);
        enemy.SetupObstacle(enemyData);

        enemy.gameObject.SetActive(true);
        enemy.StartCoroutine("FireProjectile");
    }

}
