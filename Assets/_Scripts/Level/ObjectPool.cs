// Source: https://learn.unity.com/tutorial/introduction-to-object-pooling

using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public WorldObstacle worldObstacleToPool;
    public EnemyObstacle enemyObstacleToPool;
    public Projectile projectileToPool;
    public int startingAmountToPool = 10;

    public static ObjectPool SharedInstance; // Singleton Reference
    public List<WorldObstacle> pooledWorldObstacles;
    public List<EnemyObstacle> pooledEnemyObstacles;
    public List<Projectile> pooledProjectiles;

    private int currentWorldObstaclesInPool; // How many World Obstacles are in the pool
    private int currentEnemyObstaclesInPool; // How many Enemy Obstacles are in the pool
    private int currentProjectilesInPool;    // How many Projectiles are in its pool

    private void Awake()
    {
        SharedInstance = this;
        currentWorldObstaclesInPool = startingAmountToPool;
        currentEnemyObstaclesInPool = startingAmountToPool;
        currentProjectilesInPool    = startingAmountToPool;

        // Populate the Object Pools
        pooledWorldObstacles = new List<WorldObstacle>();
        pooledEnemyObstacles = new List<EnemyObstacle>();
        pooledProjectiles    = new List<Projectile>();

        for (int i = 0; i < startingAmountToPool; i++)
        {
            // Add to Obstacle pool
            CreateNewWorldObstacle();
            CreateNewEnemyObstacle();
            CreateNewProjectile();
        }      
    }

    private WorldObstacle CreateNewWorldObstacle()
    {
        if (worldObstacleToPool == null)
            return null;
        
        WorldObstacle newObstacleObject;
        newObstacleObject = Instantiate(worldObstacleToPool);
        newObstacleObject.gameObject.SetActive(false);
        pooledWorldObstacles.Add(newObstacleObject);
        currentWorldObstaclesInPool = pooledWorldObstacles.Count;
        return newObstacleObject;     
    }

    private EnemyObstacle CreateNewEnemyObstacle()
    {
        if (enemyObstacleToPool == null)
            return null;

        EnemyObstacle newEnemyObstacle;
        newEnemyObstacle = Instantiate(enemyObstacleToPool);
        newEnemyObstacle.gameObject.SetActive(false);
        pooledEnemyObstacles.Add(newEnemyObstacle);
        currentEnemyObstaclesInPool = pooledEnemyObstacles.Count;
        return newEnemyObstacle;
    }

    private Projectile CreateNewProjectile()
    {
        if (projectileToPool == null)
            return null;
        
        Projectile newProjectileObject;
        newProjectileObject = Instantiate(projectileToPool);
        newProjectileObject.gameObject.SetActive(false);
        pooledProjectiles.Add(newProjectileObject);
        currentProjectilesInPool = pooledProjectiles.Count;
        return newProjectileObject;
        
    }

    // Attempts to retrieve an inactive World Obstacle from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetWorldObstacleObject();"
    public WorldObstacle GetWorldObstacleObject()
    {
        // Search the array for an existing inactive Obstacle
        // Returns early if it finds one
        for (int i = 0; i < currentWorldObstaclesInPool; i++)
        {
            if(!pooledWorldObstacles[i].gameObject.activeInHierarchy)
            {
                return pooledWorldObstacles[i];
            }
        }

        // If no inactive Obstacles found, attempt to create a new Obstacle
        WorldObstacle newObstacleObject = CreateNewWorldObstacle();
        if (newObstacleObject != null)
            return newObstacleObject;
        else
            return null;
    }

    // Attempts to retrieve an inactive Enemy Obstacle from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetEnemyObstacleObject();"
    public EnemyObstacle GetEnemyObstacleObject()
    {
        // Search the array for an existing inactive Obstacle
        // Returns early if it finds one
        for (int i = 0; i < currentEnemyObstaclesInPool; i++)
        {
            if (!pooledEnemyObstacles[i].gameObject.activeInHierarchy)
            {
                return pooledEnemyObstacles[i];
            }
        }

        // If no inactive Enemy Obstacles found, attempt to create a new one
        EnemyObstacle newEnemyObstacle = CreateNewEnemyObstacle();
        if (newEnemyObstacle != null)
            return newEnemyObstacle;
        else
            return null;
    }

    // Attempts to retrieve an inactive Projectile from the pool
    // Call this from any class using "ObjectPool.SharedInstance.GetProjectileObject();"
    public Projectile GetProjectileObject()
    {
        // Search the array for an existing inactive Projectile
        // Returns early if it finds one
        for (int i = 0; i < currentProjectilesInPool; i++)
        {
            if (!pooledProjectiles[i].gameObject.activeInHierarchy)
            {
                return pooledProjectiles[i];
            }
        }

        // If no inactive Projectiles found, attempt to create a new Projectile
        Projectile newProjectileObject = CreateNewProjectile();
        if (newProjectileObject != null)
            return newProjectileObject;
        else
            return null;
    }

    // Sets all Pooled Objects to inactive, to be used again later.
    public void DisableAllPooledObjects()
    {       
        foreach (var item in pooledWorldObstacles)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in pooledEnemyObstacles)
        {
            item.gameObject.SetActive(false);
        }

        foreach (var item in pooledProjectiles)
        {
            item.gameObject.SetActive(false);
        }
    }
}
