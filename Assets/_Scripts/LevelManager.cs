using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public float spawnZPoint = 30.0f; // the Z position objects should spawn
    public float despawnZPoint = -10.0f; // the z position object despawn
    public float worldMoveSpeed = 10.0f; // how fast this object moves towards the camera

    private IEnumerator SpawnRocks()
    {
        print("Started SpawnRock CoRoutine");
        GameObject gameobj = ObjectPool.SharedInstance.GetPooledObject();
        WorldObstacle obstacle = gameobj.GetComponent<WorldObstacle>();
        if(obstacle != null)
        {
            float randX = Random.Range(-5.0f, 5.0f);
            obstacle.transform.position = new Vector3(randX, 0.0f, spawnZPoint);
            obstacle.SetMoveSpeed(worldMoveSpeed);
            obstacle.SetDeactivatePoint(despawnZPoint);

            gameobj.SetActive(true);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(SpawnRocks());
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnRocks());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
