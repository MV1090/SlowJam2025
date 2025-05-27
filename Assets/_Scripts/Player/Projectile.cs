using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 2f;
    public Vector3 targetPosition;

    private Vector3 moveDirection;

    private void Start()
    {
        // Calculate direction once
        moveDirection = (targetPosition - transform.position).normalized;

        // Destroy after lifespan (optional)
        Destroy(gameObject, lifeSpan);
    }

    private void Update()
    {
        // Move in the direction every frame
        transform.position += moveDirection * projectileSpeed * Time.deltaTime;

        // Keep Z fixed to simulate 2D behavior in a 3D space (optional)
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
}
