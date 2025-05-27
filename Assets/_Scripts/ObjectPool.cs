// Source: https://learn.unity.com/tutorial/introduction-to-object-pooling

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public GameObject projectileToPool;
    public int amountToPool;
    public static ObjectPool SharedInstance; // Other scripts call this to retrieve a pooled object
    public List<GameObject> pooledObjects;
    public List<GameObject> pooledProjectiles;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Populate the Object Pools
        pooledObjects = new List<GameObject>();
        GameObject tmp;
        GameObject tmpProj;

        for(int i = 0; i < amountToPool; i++)
        {
            // Add to Obstacle pool
            if (objectToPool != null)
            {
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);
                pooledObjects.Add(tmp);
            }

            // Add to Projectile pool
            if (projectileToPool != null)
            {
                tmpProj = Instantiate(projectileToPool);
                tmpProj.SetActive(false);
                pooledProjectiles.Add(tmpProj);
            }
        }
    }

    // Attempts to retrieve an inactive object from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetPooledObject();"
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < amountToPool; i++)
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
    public GameObject GetProjectileObject()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            if (!pooledProjectiles[i].activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }

        return null;
    }
}
