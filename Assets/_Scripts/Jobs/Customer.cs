using System;
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

    [SerializeField]private int totalWallet;
    private int remainingWallet;

    void Start()
    {
        playerPosOffset = new Vector3(0, 0, 0.1f);

        isPassenger = false;

        pickedUP += OnPickUP;
        droppedOff += OnDropOff;
        receivedFood += OnReceivedFood;

        rb = GetComponent<Rigidbody>();

        //Just here for testing.
        remainingWallet = totalWallet;
    }
    
    void Update()
    {
        if(isPassenger)
        {
            transform.position = playerPos.position + playerPosOffset;
        }
    }

    //To be used when a pooled customer gets reused. 
    //private void OnEnable()
    //{
    //    remainingWallet = totalWallet;
    //}
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
            GameManager.Instance.money += remainingWallet;
            Debug.Log("Customer at Stop");
        }        
    }
    void OnReceivedFood() 
    {
        GameManager.Instance.money += remainingWallet;
        remainingWallet = 0;
        Debug.Log("Food Received");
    }

    void OnTakeDamage(int damage, int moneyLost)
    {
        //customer looses health and gets less money.
        moneyLost = damage / 2;
        remainingWallet -= moneyLost;
    }
}
