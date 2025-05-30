// Source: https://learn.unity.com/tutorial/introduction-to-object-pooling

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int objectAmountToPool = 10;
    public Projectile projectileToPool;
    public int projectileAmountToPool = 25;

    public static ObjectPool SharedInstance; // Other scripts call this to retrieve a pooled object
    public List<GameObject> pooledObjects;
    public List<Projectile> pooledProjectiles;

    private int currentObstaclesInPool; // Stores how many Obstacles are in its pool

    private void Awake()
    {
        SharedInstance = this;
        currentObstaclesInPool = objectAmountToPool;

        // Populate the Object Pools
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        Projectile tmpProj;

        for (int i = 0; i < objectAmountToPool; i++)
        {
            // Add to Obstacle pool
            CreateNewObstacle();
        }

        for (int i = 0; i < projectileAmountToPool; i++)
        {
            // Add to Projectile pool
            if (projectileToPool != null)
            {
                tmpProj = Instantiate(projectileToPool);
                tmpProj.gameObject.SetActive(false);
                pooledProjectiles.Add(tmpProj);
            }
        }

    }

    void Start()
    {
        
    }

    private GameObject CreateNewObstacle()
    {
        if (objectToPool != null)
        {
            GameObject newObstacleObject;
            newObstacleObject = Instantiate(objectToPool);
            newObstacleObject.SetActive(false);
            pooledObjects.Add(newObstacleObject);
            currentObstaclesInPool = pooledObjects.Count;
            return newObstacleObject;
        }
        return null;
    }

    // Attempts to retrieve an inactive object from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetObstacleObject();"
    public GameObject GetObstacleObject()
    {
        // Search the array for an existing inactive Obstacle
        for (int i = 0; i < currentObstaclesInPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        // Attempt to create a new Obstacle
        GameObject newObstacleObject = CreateNewObstacle();
        if (newObstacleObject != null)
            return newObstacleObject;
        else
            return null;
    }

    // Attempts to retrieve an inactive Projectile from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetProjectileObject();"
    public Projectile GetProjectileObject()
    {
        for (int i = 0; i < projectileAmountToPool; i++)
        {
            if (!pooledProjectiles[i].gameObject.activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }

        return null;
    }
}
