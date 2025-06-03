using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/** Level Manager
 *  Responsible for spawning objects into the level. It uses LevelEncounterScripableObjects to spawn its objects in groups.
 *  In between Level Encounters, it spawns random objects using ObstacleScriptableObjects.
 *  Once a certain amount of Level Encounters are spawned, the level is considered complete and will spawn a Level Exit.
 *  If a job is accepted, the Level Manager will spawn those periodically on top of the Encounters and random objects.
 */

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

    [Header("Level Encounters")]
    [Tooltip("How many encounters should spawn this level?")]
    public int encountersInLevel = 5;

    [Tooltip("How quickly do new Encounters occur during the level (in seconds).")]
    public float encounterRate = 5.0f;

    [Tooltip("Lists what kind of Encounters can appear in this level randomly.")]
    public List<LevelEncounterScriptableObject> encounterList;

    [Tooltip("Lists what kind of Obstacles can spawn between Encounters in this level.")]
    public List<ObstacleScriptableObject> obstaclesList;

    private int levelsCompleted = 0;
    private int encountersCompleted = 0;    

    //public List<JobEncounterScriptableObject> jobEncounterList;
    //public LevelEncounterScriptableObject endLevelEncounter;

    private int obstacleSpawnCounter; // during an encounter, tracks how many obstacles are left to spawn
    private bool isDoingEncounter = false; // is the level currently doing an encounter?
    
    private JobManager jobManager;
    public Customer customerPrefab;
    public GameObject taxiStopPrefab;
    public GameObject trashPrefab;
    public GameObject levelExitPrefab;
    private bool isSpawningStop = true;

    [HideInInspector]
    public static LevelManager LevelInstance; // Static Singleton referenced by other objects

    [HideInInspector]
    public GameObject playerRef;

    private IEnumerator SpawnRandomObstacle()
    {
        if (isDoingEncounter == false && encountersCompleted < encountersInLevel)
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

        if (obstacleSpawnCounter > 0) // continue spawning the obstacles it needs
        {
            obstacleSpawnCounter--;
            StartCoroutine(DoEncounter(levelEncounter));
        }
        else // the encounter is completed, run cooldown and then pick a new encounter
        {
            encountersCompleted++;
            isDoingEncounter = false;
            print("Encounter Completed: " + encountersCompleted + "/" + encountersInLevel);

            yield return new WaitForSeconds(encounterRate);
            StartCoroutine(ChooseNewEncounter());
        }
    }

    private IEnumerator ChooseNewEncounter()
    {
        if (encounterList.Count == 0)
            yield break;
        
        if (encountersCompleted >= encountersInLevel) // spawn the exit
        {
            GameObject levelExit = Instantiate(levelExitPrefab);
            levelExit.transform.position = new Vector3(0.0f, 0.0f, maxBoundary);            
        }
        else // pick a new encounter to spawn
        {
            //print("Selecting a new Encounter...");
            int randI = Random.Range(0, encounterList.Count);
            LevelEncounterScriptableObject selectedEncounter = encounterList[randI];

            obstacleSpawnCounter = selectedEncounter.amountToSpawn;
            StartCoroutine(DoEncounter(selectedEncounter));
            isDoingEncounter = true;
        }
               
        //yield return new WaitForSeconds(encounterTimer);
        //StartCoroutine(ChooseNewEncounter());
    }

    private IEnumerator BeginJob()
    {       
        yield return new WaitForSeconds(3.0f);
        if (jobManager)
        {
            if (jobManager.currentJob != null)
            {
                jobManager.currentJob.StartJob();
                print("Your assigned Job has Started!");

                if (jobManager.currentJob.jobState == JobManager.JobState.Unemployed)
                {
                    print("No Jobs yet.");
                    yield break;
                }

                if (jobManager.currentJob.jobState == JobManager.JobState.Delivery)
                {
                    print("Deliver those pizzas!");
                }
                else if (jobManager.currentJob.jobState == JobManager.JobState.TaxiDriver)
                {
                    print("Get some Taxi fares!");
                }
                else if (jobManager.currentJob.jobState == JobManager.JobState.Sweeper)
                {
                    print("Sweep the streets!");
                }

                StartCoroutine(SpawnJobObstacle());
            }
        }
        else
        {
            print("WARNING: Level Manager failed to find Job Manager in its GameObject. Jobs won't work!");
        }
    }

    private IEnumerator SpawnJobObstacle()
    {
        if (jobManager.currentJob.jobState == JobManager.JobState.Delivery)
        {
            print("Someone's waiting for their delivery!");

            Customer newCustomer = Instantiate(customerPrefab);
            newCustomer.transform.position = new Vector3(5.0f, 0.0f, maxBoundary);
            //newCustomer.SetMoveSpeed(worldMoveSpeed);
            //newCustomer.SetDeactivatePoint(minBoundary);
        }
        else if(jobManager.currentJob.jobState == JobManager.JobState.TaxiDriver)
        {
            isSpawningStop = !isSpawningStop; // toggle between spawning customers and stops

            if (isSpawningStop)
            {
                print("Drop off that person!");
                GameObject newStop = Instantiate(taxiStopPrefab);
                //newStop.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                float randX = Random.Range(-1.0f, 1.0f);
                newStop.transform.position = new Vector3(randX, 0.0f, maxBoundary);
            }
            else
            {
                print("Pick up that person!");
                Customer newCustomer = Instantiate(customerPrefab);
                newCustomer.gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

                float randX = Random.Range(-1.0f, 1.0f);
                newCustomer.transform.position = new Vector3(randX, 0.0f, maxBoundary);
            }
        }
        else if (jobManager.currentJob.jobState == JobManager.JobState.Sweeper)
        {
            print("Trash on the street!");
            GameObject newTrash = Instantiate(trashPrefab);
            //newStop.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);

            float randX = Random.Range(-1.0f, 1.0f);
            newTrash.transform.position = new Vector3(randX, 0.0f, maxBoundary);
        }

        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SpawnJobObstacle());
    }

    private void Awake()
    {
        LevelInstance = this;
        jobManager = GetComponent<JobManager>();
        playerRef = GameObject.FindGameObjectWithTag("Player"); // store player reference with level

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUpNewLevel();

    }

    public void SetUpNewLevel()
    {       
        encountersCompleted = 0;

        StopAllCoroutines();
        StartCoroutine(SpawnRandomObstacle());
        StartCoroutine(ChooseNewEncounter());
        StartCoroutine(BeginJob());
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
        
        enemy.transform.position = new Vector3(randX, spawnY, maxBoundary);

        enemy.SetMoveSpeed(worldMoveSpeed);
        enemy.SetDeactivatePoint(minBoundary);
        enemy.SetupObstacle(enemyData);

        enemy.gameObject.SetActive(true);
        enemy.StartCoroutine("FireProjectile");
    }

}
