using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource; // Reference to the manually added AudioSource
    [SerializeField] private AudioClip backgroundTrack; // Reference to the background track
    [SerializeField] private AudioClip mainMenuTrack; // Reference to the main menu track
    [SerializeField] private AudioClip[] shootingSoundEffects; // Array to hold multiple shooting sound effects
    [SerializeField] private AudioClip pizzaThrowSoundEffect; // Reference to the Pizza Throw sound effect
    [SerializeField] private AudioSource soundEffectsSource; // Separate AudioSource for sound effects
    [SerializeField] private AudioClip jetpackOnSoundEffect; // Reference to the Jetpack On sound effect
    [SerializeField] private AudioClip footstepsSoundEffect; // Reference to the Footsteps sound effect
    [SerializeField] private AudioClip explosionSoundEffect; // Reference to the Explosion sound effect
    [SerializeField] private AudioSource movementSoundSource; // Separate AudioSource for movement sounds
    [SerializeField] private AudioClip foodReceivedSoundEffect; // Reference to the Food Received sound effect
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
        if (soundEffectsSource != null && shootingSoundEffects.Length > 0)
        {
            // Get the current clip based on the tracker
            AudioClip clip = shootingSoundEffects[currentShootingClipIndex];

            // Set the source of the clip and play it
            soundEffectsSource.clip = clip;
            soundEffectsSource.volume =  0.3f;
            soundEffectsSource.Play();

            // Update the tracker to point to the next clip
            currentShootingClipIndex = (currentShootingClipIndex + 1) % shootingSoundEffects.Length;
        }
    }

    public void PlayPizzaThrowSoundEffect()
    {
        if (soundEffectsSource != null && pizzaThrowSoundEffect != null)
        {
            // Set the source of the clip and play it
            soundEffectsSource.clip = pizzaThrowSoundEffect;
            soundEffectsSource.volume = 0.7f;
            soundEffectsSource.Play();
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
        if (soundEffectsSource != null && explosionSoundEffect != null)
        {
            soundEffectsSource.clip = explosionSoundEffect;
            soundEffectsSource.volume = 0.7f;
            soundEffectsSource.Play();
        }
    }

    public void PlayFoodReceivedSoundEffect()
    {
        if (soundEffectsSource != null && foodReceivedSoundEffect != null)
        {
            soundEffectsSource.clip = foodReceivedSoundEffect;
            soundEffectsSource.volume = 0.7f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger1StartSoundEffect()
    {
        if (soundEffectsSource != null && passenger1StartSoundEffect != null)
        {
            soundEffectsSource.clip = passenger1StartSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger1CompleteSoundEffect()
    {
        if (soundEffectsSource != null && passenger1CompleteSoundEffect != null)
        {
            soundEffectsSource.clip = passenger1CompleteSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger2StartSoundEffect()
    {
        if (soundEffectsSource != null && passenger2StartSoundEffect != null)
        {
            soundEffectsSource.clip = passenger2StartSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger2CompleteSoundEffect()
    {
        if (soundEffectsSource != null && passenger2CompleteSoundEffect != null)
        {
            soundEffectsSource.clip = passenger2CompleteSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger3StartSoundEffect()
    {
        if (soundEffectsSource != null && passenger3StartSoundEffect != null)
        {
            soundEffectsSource.clip = passenger3StartSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger3CompleteSoundEffect()
    {
        if (soundEffectsSource != null && passenger3CompleteSoundEffect != null)
        {
            soundEffectsSource.clip = passenger3CompleteSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger4StartSoundEffect()
    {
        if (soundEffectsSource != null && passenger4StartSoundEffect != null)
        {
            soundEffectsSource.clip = passenger4StartSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger4CompleteSoundEffect()
    {
        if (soundEffectsSource != null && passenger4CompleteSoundEffect != null)
        {
            soundEffectsSource.clip = passenger4CompleteSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger5StartSoundEffect()
    {
        if (soundEffectsSource != null && passenger5StartSoundEffect != null)
        {
            soundEffectsSource.clip = passenger5StartSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
        }
    }

    public void PlayPassenger5CompleteSoundEffect()
    {
        if (soundEffectsSource != null && passenger5CompleteSoundEffect != null)
        {
            soundEffectsSource.clip = passenger5CompleteSoundEffect;
            soundEffectsSource.volume = 0.6f;
            soundEffectsSource.Play();
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
}
