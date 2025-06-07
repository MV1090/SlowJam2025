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
    private float baseWorldSpeed;

    [Tooltip("How many encounters should spawn this level?")]
    public int encountersInLevel = 5;
    private float baseEncounters;

    [Tooltip("How quickly do new Encounters occur during the level (in seconds).")]
    public float encounterRate = 5.0f;
    private float baseEncounterRate;

    public float levelEnemyFireRate = 1.0f;
    private float baseEnemyFireRate;

    [Header("Level Encounters")]
    [Tooltip("Stores every possible level this Manager can pick from.")]
    public List<LevelDetailsScriptableObject> gameLevels; // all game levels available
    private LevelDetailsScriptableObject currentLevel; // current level to use encounters/obstacles from

    //[Tooltip("Lists what kind of Encounters can appear in this level randomly.")]
    //private List<LevelEncounterScriptableObject> encounterList;

    //[Tooltip("Lists what kind of Obstacles can spawn between Encounters in this level.")]
    //private List<ObstacleScriptableObject> obstaclesList;

    private int encountersCompleted = 0;

    private int obstacleSpawnCounter; // during an encounter, tracks how many obstacles are left to spawn
    private bool isDoingEncounter = false; // is the level currently doing an encounter? 

    [Header("Level Prefab References")]
    private JobManager jobManager;
    public GameObject jobParentGroup;
    public Customer customerPrefab;
    public GameObject taxiStopPrefab;
    public GameObject trashPrefab;
    public GameObject levelExitPrefab;
    private bool isSpawningStop = false;

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

                int randI = Random.Range(0, currentLevel.levelRandomObstacles.Count);
                obstacle.SetupObstacle(currentLevel.levelRandomObstacles[randI]);

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
                CreateEnemyObstacle((EnemyScriptableObject)levelEncounter.objectToSpawn);
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
        if (currentLevel.levelEncounters.Count == 0)
            yield break;
        
        if (encountersCompleted >= encountersInLevel) // spawn the exit
        {
            if (isSpawningStop) // wait and try again
            {
                yield return new WaitForSeconds(encounterRate);
                StartCoroutine(ChooseNewEncounter());
            }
            else
            {
                GameObject levelExit = Instantiate(levelExitPrefab);
                SetupTransformForSpecialObject(levelExit.transform, 0.0f);
            }
        }
        else // pick a new encounter to spawn
        {
            //print("Selecting a new Encounter...");
            int randI = Random.Range(0, currentLevel.levelEncounters.Count);
            LevelEncounterScriptableObject selectedEncounter = currentLevel.levelEncounters[randI];

            obstacleSpawnCounter = selectedEncounter.amountToSpawn;
            StartCoroutine(DoEncounter(selectedEncounter));
            isDoingEncounter = true;
        }
               
        //yield return new WaitForSeconds(encounterTimer);
        //StartCoroutine(ChooseNewEncounter());
    }

    private IEnumerator BeginJob()
    {       
        if(jobManager == null)
        {
            print("WARNING: Level Manager failed to find Job Manager in its GameObject. Jobs won't work!");
            yield break;
        }
        
        yield return new WaitForSeconds(3.0f);
  
        jobManager.ChooseRandomJob(); // DEBUG: Shuffle Jobs randomly.
        
        jobManager.currentJob.StartJob();
        //print("Your assigned Job has Started!");

        if (jobManager.currentJob.jobState == JobManager.JobState.Unemployed)
        {
            GameManager.Instance.ChangeJobDescription("---");
            yield break;
        }

        if (jobManager.currentJob.jobState == JobManager.JobState.Delivery)
        {
            GameManager.Instance.ChangeJobDescription("Deliver Pizza!");

        }
        else if (jobManager.currentJob.jobState == JobManager.JobState.TaxiDriver)
        {
            GameManager.Instance.ChangeJobDescription("Taxi Customers!");

        }
        else if (jobManager.currentJob.jobState == JobManager.JobState.Sweeper)
        {
            GameManager.Instance.ChangeJobDescription("Sweep Trash!");

        }

        StartCoroutine(SpawnJobObstacle());             
        
    }

    private IEnumerator SpawnJobObstacle()
    {
        if (jobManager.currentJob.jobState == JobManager.JobState.Delivery)
        {
            // Do NOT continue if all encounters are completed (end of level)
            if (encountersCompleted >= encountersInLevel)
                yield break;

            // Spawn a new customer on the edges of the level boundary to receive food
            Customer newCustomer = Instantiate(customerPrefab);
            if (currentLevel.levelCustomers)
            {
                newCustomer.GetComponent<WorldObstacle>().SetupObstacle(currentLevel.levelCustomers);
            }

            // Randomly choose between the left or right side
            if(Random.value <= 0.5)
                SetupTransformForSpecialObject(newCustomer.transform, XPosSpawnVariance);
            else
                SetupTransformForSpecialObject(newCustomer.transform, -XPosSpawnVariance);            
 
        }
        else if(jobManager.currentJob.jobState == JobManager.JobState.TaxiDriver)
        {         
            if (isSpawningStop)
            {
                // Spawn a taxi stop for the fare
                GameObject newStop = Instantiate(taxiStopPrefab);               

                float randX = Random.Range(-1.0f, 1.0f);
                SetupTransformForSpecialObject(newStop.transform, randX);
            }
            else
            {
                // Do NOT continue if all encounters are completed (end of level)
                if (encountersCompleted >= encountersInLevel)
                    yield break;

                // Spawn a new customer to be picked up
                Customer newCustomer = Instantiate(customerPrefab);
                if (currentLevel.levelCustomers)
                {
                    newCustomer.GetComponent<WorldObstacle>().SetupObstacle(currentLevel.levelCustomers);
                }

                float randX = Random.Range(-1.0f, 1.0f);
                SetupTransformForSpecialObject(newCustomer.transform, randX);               
            }

            // toggle between spawning customers and stops
            // TODO: Should only spawn stops if a fare is picked up; fares otherwise
            isSpawningStop = !isSpawningStop; 

        }
        else if (jobManager.currentJob.jobState == JobManager.JobState.Sweeper)
        {
            // Do NOT continue if all encounters are completed (end of level)
            if (encountersCompleted >= encountersInLevel)
                yield break;

            // Spawn a new trash object to be swept up
            GameObject newTrash = Instantiate(trashPrefab);

            float randX = Random.Range(-1.0f, 1.0f);
            SetupTransformForSpecialObject(newTrash.transform, randX);
        }

        yield return new WaitForSeconds(5.0f);
        StartCoroutine(SpawnJobObstacle());
    }

    private void Awake()
    {
        LevelInstance = this;
        jobManager = GetComponent<JobManager>();
        playerRef = GameObject.FindGameObjectWithTag("Player"); // store player reference with level

        // Store current values as the base difficulty
        baseEncounterRate = encounterRate;
        baseEncounters = encountersInLevel;
        baseWorldSpeed = worldMoveSpeed;
        baseEnemyFireRate = levelEnemyFireRate;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //SetUpNewLevel();

    }

    public void StartNewGame()
    {
        ClearAllObstacles();
        GameManager.Instance.Level = 0;
        SetUpNewLevel();

    }

    public void SetUpNewLevel()
    {        
        StopAllCoroutines(); // stop all timers immediately

        // Reset certain values
        encountersCompleted = 0;
        isSpawningStop = false;
        GameManager.Instance.ChangeJobDescription("---");

        // Update scaling
        GameManager.Instance.Level++;
        int currentStage = GameManager.Instance.Level;
        worldMoveSpeed = Mathf.Min(baseWorldSpeed + (currentStage/2), baseWorldSpeed * 2);
        encounterRate = Mathf.Max(baseEncounterRate - (currentStage * 0.1f), baseEncounterRate/2);
        encountersInLevel = Mathf.RoundToInt(baseEncounters + (currentStage/2));
        levelEnemyFireRate = Mathf.Max(baseEnemyFireRate - (currentStage * 0.05f), baseEnemyFireRate / 2);

        SelectLevel();
       
        StartCoroutine(SpawnRandomObstacle());
        StartCoroutine(ChooseNewEncounter());
        StartCoroutine(BeginJob());

    }

    public void SelectLevel()
    {
        int randI = Random.Range(0, gameLevels.Count);
        currentLevel = gameLevels[randI];
    }

    public void ClearAllObstacles()
    {
        DestroyAllUnPooledObjects();
        ObjectPool.SharedInstance.DisableAllPooledObjects();
        print("Obstacles cleared by Level Manager.");
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

    private void CreateEnemyObstacle(EnemyScriptableObject enemyData)
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
        enemy.projectileSpeed = Mathf.Round(worldMoveSpeed/2);
        enemy.fireRate = levelEnemyFireRate;
        enemy.SetupEnemyObstacle(enemyData);

        enemy.gameObject.SetActive(true);
        enemy.StartCoroutine("FireProjectile");
    }

    // Helper function to set up an unpooled GameObject instantiated by this class
    // Sets position in level, scale, and adds it to a parent GameObject for later reference
    private void SetupTransformForSpecialObject(Transform obj, float xPos, float yPos = 0.0f, float scale = 1.0f)
    {
        obj.position = new Vector3(xPos, yPos, maxBoundary);
        obj.localScale = new Vector3(scale, scale, 1.0f);
        obj.SetParent(jobParentGroup.transform);
    }

    public void DestroyAllUnPooledObjects()
    {
        if(jobParentGroup)
        {
            Transform[] objs = jobParentGroup.GetComponentsInChildren<Transform>();
            print(objs.Length);

            foreach (var item in objs)
            {
                Destroy(item.gameObject);
            }     
        }

        // Create a new empty GameObject for Unpooled Objects to parent to
        GameObject newParent = new GameObject();
        newParent.transform.SetParent(this.transform);
        newParent.name = "UnpooledObjects";
        jobParentGroup = newParent;


    }
}
