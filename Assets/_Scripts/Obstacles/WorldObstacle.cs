using System.Collections.Generic;
using TMPro;
using UnityEngine;

 public enum ObstacleType
{
    Obstacle, Enemy, Customer, EndLevel
}

public class WorldObstacle : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    protected float deactivateZPoint = -10.0f;
    public bool destructible = true;
    protected int hitPoints = 1;
    public bool setInactiveOnDespawn = true;

    public SpriteRenderer sprRef; 
    public ObstacleType obstacleType = ObstacleType.Obstacle;

    public GameObject floatingScorePrefab;

    public Vector3 spriteOffset;
    private BoxCollider obstacleCollider;

    private void Awake()
    {
        obstacleCollider = gameObject.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < deactivateZPoint)
        {
            if (setInactiveOnDespawn)
                gameObject.SetActive(false);
            else
                Destroy(this);
        }

        // Move towards the 0 point
        transform.Translate( -(Vector3.forward * moveSpeed) * Time.deltaTime);
        
    }

    public void SetupObstacle(ObstacleScriptableObject obstacleData)
    {
        obstacleType = obstacleData.obstacleType;
        sprRef.sprite = obstacleData.obstacleSprite;
        destructible = obstacleData.isDestructible;

        // Resize collider based on the sprite size
        obstacleCollider.size = new Vector3(obstacleData.obstacleSprite.bounds.size.x * obstacleData.obstacleScale.x, 
            obstacleData.obstacleSprite.bounds.size.y * obstacleData.obstacleScale.y, obstacleData.obstacleSprite.bounds.size.z);
        obstacleCollider.center = obstacleData.obstacleSprite.bounds.center + spriteOffset;

    }

    protected void OnTriggerEnter(Collider other)
    {
        if (obstacleType == ObstacleType.Customer)
        {   
            // Delivery: Have customer react to receiving their food
            print("Projectile hits a customer.");                    
            //onDelivered?.Invoke();            
            //Customer customer = collision.gameObject.GetComponent<Customer>();
            GetComponent<Customer>().receivedFood?.Invoke();
            gameObject.SetActive(false);
        }         
        else if (other.gameObject.CompareTag("Projectile"))
        {
            if (!destructible)
            {
                other.gameObject.SetActive(false);
                return;
            }               

            ShowScore(transform.position, 10);
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            
            GameManager.Instance.Score += 10;
        }
        //if (other.gameObject.CompareTag("Player"))
        //    print("This WorldObject hit the Player, add functionality in the future!");
        //else
        //    print("Bonk!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Crash!");
    }

    public void SetDestructible(bool newDestructible)
    {
        destructible = newDestructible;
    }

    public void SetRelativeHitPoints(int hitPointsChange)
    {
        if(!destructible) // Don't continue if this object is indestructible
        {
            return;
        }

        hitPoints += hitPointsChange;

        if(hitPoints < 1) // Deactivate this object
        {
            gameObject.SetActive(false);
        }
    }

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetDeactivatePoint(float newZ)
    {
        deactivateZPoint = newZ;
    }
    public void ShowScore(Vector3 position, int scoreValue)
    {
        GameObject instance = Instantiate(floatingScorePrefab, position, Quaternion.identity);
        instance.GetComponent<TMP_Text>().text = $"+{scoreValue}";
    }
}
