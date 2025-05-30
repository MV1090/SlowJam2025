using System;
using System.Collections;
using UnityEngine;

public class FoodBag : MonoBehaviour
{
    public float projectileSpeed = 10f;
    public float lifeSpan = 5f;
    public Vector3 direction;

    //public Action onDelivered;
    //public Action notDelivered;
    
    Customer customer;
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

        StartLifeTimer();
    }

    private void Update()
    {
        transform.Translate((direction * projectileSpeed) * Time.deltaTime, Space.World);
    }

    public void StartLifeTimer()
    {
        StartCoroutine(LifeTimer(lifeSpan));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            return;

        if (collision.gameObject.tag == "Customer")
        {
            //onDelivered?.Invoke();            
            customer = collision.gameObject.GetComponent<Customer>();
            customer.receivedFood?.Invoke();

            Destroy(gameObject);            
        }
        else
        {
            //notDelivered?.Invoke();
            Destroy(gameObject);
            Debug.Log("customer died of starvation");
        }

    }
}
