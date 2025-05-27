using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 5f;
    public Vector3 direction;


    public Rigidbody rb;

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
}
