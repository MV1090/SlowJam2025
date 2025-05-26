using UnityEngine;

public class WorldObstacle : MonoBehaviour
{
    private float moveSpeed = 10.0f;
    private float deactivateZPoint = -10.0f;

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

    public void SetMoveSpeed(float newMoveSpeed)
    {
        moveSpeed = newMoveSpeed;
    }

    public void SetDeactivatePoint(float newZ)
    {
        deactivateZPoint = newZ;
    }
}
