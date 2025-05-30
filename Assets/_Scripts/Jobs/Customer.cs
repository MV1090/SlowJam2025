using System;
using Unity.VisualScripting;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public Action pickedUP;
    public Action droppedOff;
    public Action receivedFood;

    public bool isPassenger;

    public Transform playerPos;
    Vector3 playerPosOffset;
    
    private Rigidbody rb;

    void Start()
    {
        playerPosOffset = new Vector3(0, 0, 0.1f);

        isPassenger = false;

        pickedUP += OnPickUP;
        droppedOff += OnDropOff;
        receivedFood += OnReceivedFood;

        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if(isPassenger)
        {
            transform.position = playerPos.position + playerPosOffset;
        }
    }
    private void OnPickUP() 
    {           
        isPassenger = true;
    }
    private void OnDropOff()
    {
        isPassenger = false;

        int StopLayer = 1 << LayerMask.NameToLayer("StopSign");

        Collider[] hitCollider = Physics.OverlapBox(transform.position, new Vector3(2,2,2), transform.rotation, StopLayer);

        foreach (var hit in hitCollider)
        {            
            Debug.Log("Customer at Stop");
        }

        //rb.useGravity = true;
    }
    void OnReceivedFood() 
    {
        Debug.Log("Food Received");
    }
}
