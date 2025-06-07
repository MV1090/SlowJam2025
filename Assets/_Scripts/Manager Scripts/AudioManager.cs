using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource; // Reference to the manually added AudioSource
    [SerializeField] private AudioClip backgroundTrack; // Reference to the background track
    [SerializeField] private AudioClip mainMenuTrack; // Reference to the main menu track


    [SerializeField] private AudioClip[] shootingSoundEffects; // Array to hold multiple shooting sound effects    
    [SerializeField] private AudioSource projectileEffectsSource; // Separate AudioSource for sound effects

    [SerializeField] private AudioClip foodReceivedSoundEffect; // Reference to the Food Received sound effect
    [SerializeField] private AudioClip pizzaThrowSoundEffect; // Reference to the Pizza Throw sound effect
    [SerializeField] private AudioSource pizzaEffectSource;

    [SerializeField] private AudioClip explosionSoundEffect; // Reference to the Explosion sound effect
    [SerializeField] private AudioSource explosionEffectSource;

    [SerializeField] private AudioClip jetpackOnSoundEffect; // Reference to the Jetpack On sound effect
    [SerializeField] private AudioClip footstepsSoundEffect; // Reference to the Footsteps sound effect    
    [SerializeField] private AudioSource movementSoundSource; // Separate AudioSource for movement sounds


    
    [SerializeField] private AudioClip passenger1StartSoundEffect; // Reference to Passenger 1 start sound effect
    [SerializeField] private AudioClip passenger1CompleteSoundEffect; // Reference to Passenger 1 complete sound effect
    [SerializeField] private AudioClip passenger2StartSoundEffect; // Reference to Passenger 2 start sound effect
    [SerializeField] private AudioClip passenger2CompleteSoundEffect; // Reference to Passenger 2 complete sound effect
    [SerializeField] private AudioClip passenger3StartSoundEffect; // Reference to Passenger 3 start sound effect
    [SerializeField] private AudioClip passenger3CompleteSoundEffect; // Reference to Passenger 3 complete sound effect
    [SerializeField] private AudioClip passenger4StartSoundEffect; // Reference to Passenger 4 start sound effect
    [SerializeField] private AudioClip passenger4CompleteSoundEffect; // Reference to Passenger 4 complete sound effect
    [SerializeField] private AudioClip passenger5StartSoundEffect; // Reference to Passenger 5 start sound effect
    [SerializeField] private AudioClip passenger5CompleteSoundEffect; // Reference to Passenger 5 complete sound effect
    [SerializeField] private AudioSource passengerSoundSource;

    private int currentShootingClipIndex = 0; // Tracker for the current audio clip

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this GameObject persistent across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Ensure the AudioSource is assigned
        if (audioSource == null)
        {
            Debug.LogError("AudioSource is not assigned in the AudioManager!");
            return;
        }

        // Ensure the background track is assigned
        if (backgroundTrack == null)
        {
            Debug.LogError("Background track is not assigned in the AudioManager!");
            return;
        }

        // Ensure the main menu track is assigned
        if (mainMenuTrack == null)
        {
            Debug.LogError("Main menu track is not assigned in the AudioManager!");
            return;
        }

        // Configure the AudioSource for looping
        audioSource.loop = true;
    }

    private void Start()
    {
        // Play main menu music when the game starts
        AudioManager.Instance.PlayMainMenuTrack();

        GameManager.Instance.OnDeath += StopAllSFX;
    }

    public void PlayBackgroundTrack()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0.7f; // Set the volume to 0.6
            audioSource.clip = backgroundTrack; // Set the background track
            audioSource.Play(); // Play the track

        }
    }

    public void PlayMainMenuTrack()
    {
        if (audioSource != null)
        {
            audioSource.volume = 1.0f; // Set the volume to 0.6
            audioSource.clip = mainMenuTrack; // Set the main menu track
            audioSource.Play(); // Play the track
        }
    }

    public void PlayShootingSoundEffect()
    {
        if (projectileEffectsSource != null && shootingSoundEffects.Length > 0)
        {
            // Get the current clip based on the tracker
            AudioClip clip = shootingSoundEffects[currentShootingClipIndex];

            // Set the source of the clip and play it
            projectileEffectsSource.clip = clip;
            projectileEffectsSource.volume =  0.3f;
            projectileEffectsSource.Play();

            // Update the tracker to point to the next clip
            currentShootingClipIndex = (currentShootingClipIndex + 1) % shootingSoundEffects.Length;
        }
    }

    public void PlayPizzaThrowSoundEffect()
    {
        if (pizzaEffectSource != null && pizzaThrowSoundEffect != null)
        {
            // Set the source of the clip and play it
            pizzaEffectSource.clip = pizzaThrowSoundEffect;
            pizzaEffectSource.volume = 0.7f;
            pizzaEffectSource.Play();
        }
    }

    public void PlayJetpackOnSoundEffect()
    {
        if (movementSoundSource != null && jetpackOnSoundEffect != null)
        {
            movementSoundSource.clip = jetpackOnSoundEffect;
            movementSoundSource.volume = 0.5f;
            movementSoundSource.Play();
        }
    }


    public void PlayFootstepsSoundEffect()
    {
        if (movementSoundSource != null && footstepsSoundEffect != null)
        {
            movementSoundSource.clip = footstepsSoundEffect;
            movementSoundSource.volume = 0.6f;
            movementSoundSource.Play();
        }
    }

    public void PlayExplosionSoundEffect()
    {
        if (explosionEffectSource != null && explosionSoundEffect != null)
        {
            explosionEffectSource.clip = explosionSoundEffect;
            explosionEffectSource.volume = 0.7f;
            explosionEffectSource.Play();
        }
    }

    public void PlayFoodReceivedSoundEffect()
    {
        if (pizzaEffectSource != null && foodReceivedSoundEffect != null)
        {
            pizzaEffectSource.clip = foodReceivedSoundEffect;
            pizzaEffectSource.volume = 0.7f;
            pizzaEffectSource.Play();
        }
    }

    public void PlayPassenger1StartSoundEffect()
    {
        if (passengerSoundSource != null && passenger1StartSoundEffect != null)
        {
            passengerSoundSource.clip = passenger1StartSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger1CompleteSoundEffect()
    {
        if (passengerSoundSource != null && passenger1CompleteSoundEffect != null)
        {
            passengerSoundSource.clip = passenger1CompleteSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger2StartSoundEffect()
    {
        if (passengerSoundSource != null && passenger2StartSoundEffect != null)
        {
            passengerSoundSource.clip = passenger2StartSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger2CompleteSoundEffect()
    {
        if (passengerSoundSource != null && passenger2CompleteSoundEffect != null)
        {
            passengerSoundSource.clip = passenger2CompleteSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger3StartSoundEffect()
    {
        if (passengerSoundSource != null && passenger3StartSoundEffect != null)
        {
            passengerSoundSource.clip = passenger3StartSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger3CompleteSoundEffect()
    {
        if (passengerSoundSource != null && passenger3CompleteSoundEffect != null)
        {
            passengerSoundSource.clip = passenger3CompleteSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger4StartSoundEffect()
    {
        if (passengerSoundSource != null && passenger4StartSoundEffect != null)
        {
            passengerSoundSource.clip = passenger4StartSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger4CompleteSoundEffect()
    {
        if (passengerSoundSource != null && passenger4CompleteSoundEffect != null)
        {
            passengerSoundSource.clip = passenger4CompleteSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger5StartSoundEffect()
    {
        if (passengerSoundSource != null && passenger5StartSoundEffect != null)
        {
            passengerSoundSource.clip = passenger5StartSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void PlayPassenger5CompleteSoundEffect()
    {
        if (passengerSoundSource != null && passenger5CompleteSoundEffect != null)
        {
            passengerSoundSource.clip = passenger5CompleteSoundEffect;
            passengerSoundSource.volume = 0.6f;
            passengerSoundSource.Play();
        }
    }

    public void StopJetpackSoundEffect()
    {
        if (movementSoundSource != null && movementSoundSource.clip == jetpackOnSoundEffect)
        {
            movementSoundSource.Stop(); // Stop the jetpack sound effect
        }
    }

    public void StopFootstepsSoundEffect()
    {
        if (movementSoundSource != null && movementSoundSource.clip == footstepsSoundEffect)
        {
            movementSoundSource.Stop(); // Stop the footsteps sound effect
        }
    }

    public void PlayMovementSound()
    {
        movementSoundSource.Play();
    }

    public void StopAllSFX()
    {
        movementSoundSource.Stop();
        passengerSoundSource.Stop();
    }
}
