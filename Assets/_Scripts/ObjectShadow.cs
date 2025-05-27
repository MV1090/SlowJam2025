using UnityEngine;

public class ObjectShadow : MonoBehaviour
{
    public GameObject target;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(target.transform.position.x, 0.13f, target.transform.position.z);
    }
}
