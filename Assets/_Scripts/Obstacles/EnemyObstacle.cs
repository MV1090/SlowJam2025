using UnityEngine;
using System.Collections;

public class EnemyObstacle : WorldObstacle
{
    public float fireRate = 1.0f;
    public float projectileSpeed = 5.0f;

    private IEnumerator FireProjectile()
    {
        yield return new WaitForSeconds(fireRate);
        float distance = Vector3.Distance(LevelManager.LevelInstance.playerRef.transform.position, gameObject.transform.position);
        if (distance > 5.0f && gameObject.transform.position.z > 0.0f)
        {
            Projectile projectile = ObjectPool.SharedInstance.GetProjectileObject();
            Vector3 targetDirection = (LevelManager.LevelInstance.playerRef.transform.position - gameObject.transform.position).normalized;

            projectile.SetupProjectile(transform, 0.5f, moveSpeed + projectileSpeed, targetDirection, Projectile.ProjectileType.Enemy);           
        }     

        //yield return new WaitForSeconds(fireRate);
        StartCoroutine(FireProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < deactivateZPoint) // Enemy is now out-of-bounds, deactivate it
        {
            gameObject.SetActive(false);
            //float randX = Random.Range(-5.0f, 5.0f);
            //transform.position = new Vector3(randX, transform.position.y, 20.0f);
        }

        // Move towards the 0 point
        transform.Translate(-(Vector3.forward * moveSpeed) * Time.deltaTime);
    }

    public void SetupEnemyObstacle(EnemyScriptableObject obstacleData)
    {
        obstacleType = obstacleData.obstacleType;
        destructible = obstacleData.isDestructible;
        gameObject.transform.localScale = obstacleData.obstacleScale;
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.GetComponent<Animator>().runtimeAnimatorController = obstacleData.enemyAnim;
        hitPoints = obstacleData.health;
        moveSpeed += obstacleData.relativeSpeed;

        Sprite chosenSprite;
        chosenSprite = obstacleData.obstacleSprites[Random.Range(0, obstacleData.obstacleSprites.Count)];
        sprRef.sprite = chosenSprite;
        // Resize collider based on the sprite size
        obstacleCollider.size = new Vector3(chosenSprite.bounds.size.x * obstacleData.obstacleScale.x,
            chosenSprite.bounds.size.y * obstacleData.obstacleScale.y, chosenSprite.bounds.size.z);
        obstacleCollider.center = chosenSprite.bounds.center + spriteOffset;

    }

}
