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
}
