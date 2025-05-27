// Source: https://learn.unity.com/tutorial/introduction-to-object-pooling

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectToPool;
    public int amountToPool;
    public static ObjectPool SharedInstance; // Other scripts call this to retrieve a pooled object
    public List<GameObject> pooledObjects;

    private void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        // Populate the Object Pool with objectToPool
        pooledObjects = new List<GameObject>();
        GameObject tmp;

        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);
            pooledObjects.Add(tmp);
        }
    }

    // Attempts to retrieve an inactive object from the pool
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
}
