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
            GameObject gameobj = ObjectPool.SharedInstance.GetObstacleObject();
            WorldObstacle obstacle = gameobj.GetComponent<WorldObstacle>();
            if (obstacle != null)
            {
                float randX = Random.Range(-5.0f, 5.0f);
                obstacle.transform.position = new Vector3(randX, 0.0f, spawnZPoint); // 1.0f * Random.Range(0, 3.0f) for Y variance
                obstacle.SetMoveSpeed(worldMoveSpeed);
                obstacle.SetDeactivatePoint(despawnZPoint);

                int randI = Random.Range(0, obstaclesList.Count);
                obstacle.SetupObstacle(obstaclesList[randI]);

                gameobj.SetActive(true);
            }
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnRandomObstacle());
    }

    private IEnumerator DoEncounter(LevelEncounterScriptableObject encounter)
    {
        print("Spawning Object from Encounter");
        GameObject gameobj = ObjectPool.SharedInstance.GetObstacleObject();
        WorldObstacle obstacle = gameobj.GetComponent<WorldObstacle>();
        
        if (obstacle != null)
        {
            float randX = Random.Range(-5.0f, 5.0f);
            obstacle.transform.position = new Vector3(randX, 0.0f, spawnZPoint);
            obstacle.SetMoveSpeed(worldMoveSpeed);
            obstacle.SetDeactivatePoint(despawnZPoint);

            int randI = Random.Range(0, obstaclesList.Count);
            obstacle.SetupObstacle(encounter.objectToSpawn);

            gameobj.SetActive(true);
        }

        yield return new WaitForSeconds(encounter.spawnRate);
        if (obstacleSpawnCounter > 0)
        {
            obstacleSpawnCounter--;
            StartCoroutine(DoEncounter(encounter));
        }
        else
            isDoingEncounter = false;
    }

    private IEnumerator ChooseNewEncounter()
    {
        if (isDoingEncounter == false) // start a new encounter
        {
            print("Selecting a new Encounter...");
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

    
}
