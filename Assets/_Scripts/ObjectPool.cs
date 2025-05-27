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

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Populate the Object Pools
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        Projectile tmpProj;

        for (int i = 0; i < objectAmountToPool; i++)
        {
            // Add to Obstacle pool
            if (objectToPool != null)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }
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

    // Attempts to retrieve an inactive object from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetObstacleObject();"
    public GameObject GetObstacleObject()
    {
        for (int i = 0; i < objectAmountToPool; i++)
        {
            if(!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

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
