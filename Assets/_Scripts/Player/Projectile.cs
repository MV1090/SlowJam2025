using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 5f;
    public Vector3 direction;

    public Rigidbody rb;

    public enum ProjectileType
    {
        Player, Upgrade1, Upgrade2, Upgrade3, Enemy, 
    }

    private IEnumerator LifeTimer(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }

    private void Start()
    {   
        if (lifeSpan <= 0)
        {
            lifeSpan = 2;
        }      
    }

    private void Update()
    {       
        transform.Translate((direction * projectileSpeed) * Time.deltaTime);
    }

    public void StartLifeTimer()
    {
        StartCoroutine(LifeTimer(lifeSpan));
    }

    public void SetupProjectile(Transform parentTransform, float newScale, float newSpeed, Vector3 newDirection, ProjectileType projectileType /* bool isPlayerProjectile = false*/)
    {
        //if(isPlayerProjectile)
        //{          
        //    gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        //    gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        //}
        //else
        //{
        //    gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        //    gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        //}

        switch(projectileType)
        {
            case ProjectileType.Player:
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                break;  
                
            case ProjectileType.Upgrade1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                break;

            case ProjectileType.Upgrade2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                break;

            case ProjectileType.Upgrade3:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                break;

            case ProjectileType.Enemy:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
                break;
        };

            

        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        transform.SetPositionAndRotation(parentTransform.position, parentTransform.rotation);
        projectileSpeed = newSpeed;
        direction = newDirection;

        gameObject.SetActive(true);
        StartLifeTimer();
    }

}
