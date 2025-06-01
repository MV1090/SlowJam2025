using System.Collections.Generic;
using UnityEngine;

 public enum ObstacleType
{
    Obstacle, Enemy, Customer
}

public class WorldObstacle : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    protected float deactivateZPoint = -10.0f;
    protected bool destructible = true;
    protected int hitPoints = 1;

    public SpriteRenderer sprRef; 
    public ObstacleType obstacleType = ObstacleType.Obstacle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < deactivateZPoint)
        {
            gameObject.SetActive(false);
        }

        // Move towards the 0 point
        transform.Translate( -(Vector3.forward * moveSpeed) * Time.deltaTime);
        
    }

    public void SetupObstacle(ObstacleScriptableObject obstacleData)
    {
        obstacleType = obstacleData.obstacleType;
        sprRef.sprite = obstacleData.obstacleSprite;
        destructible = obstacleData.isDestructible;

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
}
