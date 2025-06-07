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

        // Just here for testing.
        remainingWallet = totalWallet;

        // Retrieve the WorldObstacle component and debug log the chosen sprite
        WorldObstacle worldObstacle = GetComponent<WorldObstacle>();
        if (worldObstacle != null && worldObstacle.sprRef != null)
        {
            Debug.Log("Chosen Sprite: " + (worldObstacle.sprRef.sprite != null ? worldObstacle.sprRef.sprite.name : "None"));
        }
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
            // Play food received sound effect
            AudioManager.Instance.PlayFoodReceivedSoundEffect();
        }        
    }
    void OnReceivedFood()
    {
        GameManager.Instance.money += remainingWallet;
        remainingWallet = 0;
        Debug.Log("Food Received");

        // Play food received sound effect
        AudioManager.Instance.PlayFoodReceivedSoundEffect();

        // Debug log the chosen sprite
        Sprite chosenSprite = GetComponent<SpriteRenderer>()?.sprite;
        Debug.Log("Chosen Sprite: " + (chosenSprite != null ? chosenSprite.name : "None"));
    }

    void OnTakeDamage(int damage, int moneyLost)
    {
        //customer looses health and gets less money.
        moneyLost = damage / 2;
        remainingWallet -= moneyLost;
    }
}
