using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 5f;
    public Vector3 direction;

    public Rigidbody rb;
    public Sprite defaultSprite;

    private SphereCollider projectileCollider;
    private float defaultRadius = 0.5f;

    public enum ProjectileType
    {
        Player, Upgrade1, Upgrade2, Upgrade3, Enemy, Food
    }

    private IEnumerator LifeTimer(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        projectileCollider = gameObject.GetComponent<SphereCollider>();
        defaultRadius = projectileCollider.radius;
    }

    private void Start()
    {
        //defaultSprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        
        if (lifeSpan <= 0)
        {
            lifeSpan = 2;
        }      
    }

    private void Update()
    {
        if (transform.position.y < 0.0f)
            gameObject.SetActive(false);
        transform.Translate((direction * projectileSpeed) * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Projectile hits something.");
        // Delivery: Have customer react to receiving their food
        if (collision.gameObject.CompareTag("Customer"))
        {
            //onDelivered?.Invoke();            
            Customer customer = collision.gameObject.GetComponent<Customer>();
            customer.receivedFood?.Invoke();

            gameObject.SetActive(false);
        }
        
    }

    public void StartLifeTimer()
    {
        StartCoroutine(LifeTimer(lifeSpan));
    }

    public void SetupProjectile(Transform parentTransform, float newScale, float newSpeed, Vector3 newDirection, ProjectileType projectileType, ObstacleScriptableObject projectileData = null)
    {       
        SpriteRenderer spriteComponent = gameObject.GetComponent<SpriteRenderer>();

        switch(projectileType)
        {            
            case ProjectileType.Player:
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                projectileCollider.radius = defaultRadius;
                spriteComponent.sprite = defaultSprite;
                break;  
                
            case ProjectileType.Upgrade1:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                projectileCollider.radius = defaultRadius;
                spriteComponent.sprite = defaultSprite;
                break;

            case ProjectileType.Upgrade2:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                projectileCollider.radius = defaultRadius;
                spriteComponent.sprite = defaultSprite;
                break;

            case ProjectileType.Upgrade3:
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
                projectileCollider.radius = defaultRadius;
                spriteComponent.sprite = defaultSprite;
                break;

            case ProjectileType.Enemy:
                gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
                projectileCollider.radius = defaultRadius / 2;
                spriteComponent.sprite = defaultSprite;
                break;

            case ProjectileType.Food: // Change the projectile sprite to the food item.
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.layer = LayerMask.NameToLayer("FoodBag");
                projectileCollider.radius = defaultRadius;

                if (projectileData != null)
                    spriteComponent.sprite = projectileData.obstacleSprite;
                else // fallback colour change            
                    spriteComponent.color = Color.cyan;                       
                
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
