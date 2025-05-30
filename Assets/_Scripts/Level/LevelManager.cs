using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public float spawnZPoint = 30.0f; // the Z position objects should spawn
    public float despawnZPoint = -10.0f; // the z position object despawn
    public float worldMoveSpeed = 10.0f; // how fast objects move towards the camera
    public float encounterRate = 5.0f; // how fast a new encounter appears (in seconds)

    [HideInInspector]
    public static LevelManager LevelInstance; // Static Singleton referenced by other objects

    [HideInInspector]
    public GameObject playerRef;

    // Obstacle Data
    public List<LevelEncounterScriptableObject> encounterList;
    public List<ObstacleScriptableObject> obstaclesList;
    private int obstacleSpawnCounter;
    private bool isDoingEncounter = false;

    private IEnumerator SpawnRandomObstacle()
    {
        if (isDoingEncounter == false)
        {
            //print("Started SpawnRock CoRoutine");
            WorldObstacle obstacle = ObjectPool.SharedInstance.GetWorldObstacleObject();
            
            if (obstacle != null)
            {
                float randX = Random.Range(-5.0f, 5.0f);
                obstacle.transform.position = new Vector3(randX, 0.0f, spawnZPoint); // 1.0f * Random.Range(0, 3.0f) for Y variance
                obstacle.SetMoveSpeed(worldMoveSpeed);
                obstacle.SetDeactivatePoint(despawnZPoint);

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
            float randX = Random.Range(-5.0f, 5.0f);
            obstacle.transform.position = new Vector3(randX, 0.0f, spawnZPoint);
            obstacle.SetMoveSpeed(worldMoveSpeed);
            obstacle.SetDeactivatePoint(despawnZPoint);           
            obstacle.SetupObstacle(obstacleData);

            obstacle.gameObject.SetActive(true);
        }
    }

    private void CreateEnemyObstacle(ObstacleScriptableObject enemyData)
    {
        EnemyObstacle enemy = ObjectPool.SharedInstance.GetEnemyObstacleObject();
        if (enemy == null)
            return;

        float randX = Random.Range(-5.0f, 5.0f);
        float spawnY = 0.0f;

        if(enemyData.isFloating)
            spawnY = 2.0f;
        
        enemy.transform.position = new Vector3(randX, spawnY, spawnZPoint*2);

        enemy.SetMoveSpeed(worldMoveSpeed);
        enemy.SetDeactivatePoint(despawnZPoint);
        enemy.SetupObstacle(enemyData);

        enemy.gameObject.SetActive(true);
        enemy.StartCoroutine("FireProjectile");
    }

}
