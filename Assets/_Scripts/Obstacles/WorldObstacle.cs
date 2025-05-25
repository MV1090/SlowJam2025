using UnityEngine;

public class WorldObstacle : MonoBehaviour
{
    public float moveSpeed = 100.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z < -10.0f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 10.0f);
        }

        transform.Translate( (Vector3.forward * moveSpeed) * Time.deltaTime);
    }
}
