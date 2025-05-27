using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 5f;
    public Vector3 direction;


    public Rigidbody rb;
    private void Start()
    {   
        if (lifeSpan <= 0)
        {
            lifeSpan = 2;
        }
                
        Destroy(gameObject, lifeSpan);       
        
    }

    private void Update()
    {       
        transform.Translate((direction * projectileSpeed) * Time.deltaTime);
    }
}
