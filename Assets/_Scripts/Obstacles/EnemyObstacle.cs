using UnityEngine;
using System.Collections;

public class EnemyObstacle : WorldObstacle
{
    public float fireRate = 1.0f;
    public float projectileSpeed = 5.0f;

    private IEnumerator FireProjectile()
    {
        float distance = Vector3.Distance(LevelManager.LevelInstance.playerRef.transform.position, gameObject.transform.position);
        if (distance > 5.0f && gameObject.transform.position.z > 0.0f)
        {
            Projectile projectile = ObjectPool.SharedInstance.GetProjectileObject();
            Vector3 targetDirection = (LevelManager.LevelInstance.playerRef.transform.position - gameObject.transform.position).normalized;

            projectile.transform.position = transform.position;
            projectile.transform.rotation = transform.rotation;
            projectile.projectileSpeed = moveSpeed + projectileSpeed;
            projectile.direction = targetDirection;
            projectile.gameObject.SetActive(true);
            projectile.StartLifeTimer();
        }

        //Projectile projectile = ObjectPool.SharedInstance.GetProjectileObject();
        //projectile.transform.position = transform.position;
        //projectile.transform.rotation = transform.rotation;
        //projectile.projectileSpeed = moveSpeed + projectileSpeed;
        //projectile.direction = Vector3.back;
        //projectile.gameObject.SetActive(true);
        //projectile.StartLifeTimer();

        yield return new WaitForSeconds(fireRate);
        StartCoroutine(FireProjectile());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(FireProjectile());
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < deactivateZPoint)
        {
            float randX = Random.Range(-5.0f, 5.0f);
            transform.position = new Vector3(randX, transform.position.y, 20.0f);
        }

        // Move towards the 0 point
        transform.Translate(-(Vector3.forward * moveSpeed) * Time.deltaTime);
    }
}
