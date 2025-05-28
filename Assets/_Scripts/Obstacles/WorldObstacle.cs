using UnityEngine;

public class WorldObstacle : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    protected float deactivateZPoint = -10.0f;
    protected bool destructible = true;
    protected int hitPoints = 1;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile"))
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.CompareTag("Player"))
            print("This WorldObject hit the Player, add functionality in the future!");
        else
            print("Bonk!");
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
