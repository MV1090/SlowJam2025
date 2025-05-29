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

    public void SetupProjectile(Transform parentTransform, float newScale, float newSpeed, Vector3 newDirection, bool isPlayerProjectile = false)
    {
        if(isPlayerProjectile)
        {          
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            gameObject.layer = LayerMask.NameToLayer("PlayerProjectile");
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            gameObject.layer = LayerMask.NameToLayer("EnemyProjectile");
        }

        gameObject.transform.localScale = new Vector3(newScale, newScale, newScale);
        transform.SetPositionAndRotation(parentTransform.position, parentTransform.rotation);
        projectileSpeed = newSpeed;
        direction = newDirection;

        gameObject.SetActive(true);
        StartLifeTimer();
    }

}
