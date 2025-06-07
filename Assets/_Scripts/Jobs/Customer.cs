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

    public static Sprite GlobalChosenSprite; // Global variable to store the chosen Sprite

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
            GlobalChosenSprite = worldObstacle.sprRef.sprite; // Store the chosen Sprite globally
            Debug.Log("Chosen Sprite: " + (GlobalChosenSprite != null ? GlobalChosenSprite.name : "None"));
        }
        PlayCustomerStartAudioEffect();
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
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    private void OnDropOff()
    {
        isPassenger = false;

        gameObject.layer = LayerMask.NameToLayer("Customer");

        int StopLayer = 1 << LayerMask.NameToLayer("StopSign");

        Collider[] hitCollider = Physics.OverlapBox(transform.position, new Vector3(2,2,2), transform.rotation, StopLayer);

        foreach (var hit in hitCollider)
        {
            GameManager.Instance.money += remainingWallet;
            Debug.Log("Customer at Stop");
            PlayCustomerCompleteAudioEffect();
        }        
    }
    void OnReceivedFood()
    {
        GameManager.Instance.money += remainingWallet;
        remainingWallet = 0;
        Debug.Log("Food Received");

        PlayCustomerCompleteAudioEffect();
    }

    void OnTakeDamage(int damage, int moneyLost)
    {
        //customer looses health and gets less money.
        moneyLost = damage / 2;
        remainingWallet -= moneyLost;
    }

    public void PlayCustomerStartAudioEffect()
    {
        if (GlobalChosenSprite != null)
        {
            string spriteName = GlobalChosenSprite.name;
            Debug.Log("Sprite Name from audio function: " + spriteName); // Log the sprite name

            switch (spriteName)
            {
                case "passenger1_0":
                    Debug.Log("Case: passenger1_0"); // Log the case
                    Debug.Log("Playing: Passenger1StartSoundEffect"); // Log the audio effect
                    AudioManager.Instance.PlayPassenger1StartSoundEffect();
                    break;
                case "passenger2_0":
                    Debug.Log("Case: passenger2_0");
                    Debug.Log("Playing: Passenger2StartSoundEffect");
                    AudioManager.Instance.PlayPassenger2StartSoundEffect();
                    break;
                case "passenger3_0":
                    Debug.Log("Case: passenger3_0");
                    Debug.Log("Playing: Passenger3StartSoundEffect");
                    AudioManager.Instance.PlayPassenger3StartSoundEffect();
                    break;
                case "passenger4_0":
                    Debug.Log("Case: passenger4_0");
                    Debug.Log("Playing: Passenger4StartSoundEffect");
                    AudioManager.Instance.PlayPassenger4StartSoundEffect();
                    break;
                case "passenger5_0":
                    Debug.Log("Case: passenger5_0");
                    Debug.Log("Playing: Passenger5StartSoundEffect");
                    AudioManager.Instance.PlayPassenger5StartSoundEffect();
                    break;
                default:
                    Debug.Log("No matching audio effect for sprite: " + spriteName);
                    break;
            }
        }
        else
        {
            Debug.Log("GlobalChosenSprite is null, cannot play audio effect.");
        }
    }

    public void PlayCustomerCompleteAudioEffect()
    {
        if (GlobalChosenSprite != null)
        {
            string spriteName = GlobalChosenSprite.name;
            Debug.Log("Sprite Name from complete audio function: " + spriteName); // Log the sprite name

            switch (spriteName)
            {
                case "passenger1_0":
                    Debug.Log("Case: passenger1_0"); // Log the case
                    Debug.Log("Playing: Passenger1CompleteSoundEffect"); // Log the audio effect
                    AudioManager.Instance.PlayPassenger1CompleteSoundEffect();
                    break;
                case "passenger2_0":
                    Debug.Log("Case: passenger2_0");
                    Debug.Log("Playing: Passenger2CompleteSoundEffect");
                    AudioManager.Instance.PlayPassenger2CompleteSoundEffect();
                    break;
                case "passenger3_0":
                    Debug.Log("Case: passenger3_0");
                    Debug.Log("Playing: Passenger3CompleteSoundEffect");
                    AudioManager.Instance.PlayPassenger3CompleteSoundEffect();
                    break;
                case "passenger4_0":
                    Debug.Log("Case: passenger4_0");
                    Debug.Log("Playing: Passenger4CompleteSoundEffect");
                    AudioManager.Instance.PlayPassenger4CompleteSoundEffect();
                    break;
                case "passenger5_0":
                    Debug.Log("Case: passenger5_0");
                    Debug.Log("Playing: Passenger5CompleteSoundEffect");
                    AudioManager.Instance.PlayPassenger5CompleteSoundEffect();
                    break;
                default:
                    Debug.Log("No matching audio effect for sprite: " + spriteName);
                    break;
            }
        }
        else
        {
            Debug.Log("GlobalChosenSprite is null, cannot play audio effect.");
        }
    }
}
