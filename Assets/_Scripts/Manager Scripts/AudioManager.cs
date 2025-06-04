using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource; // Reference to the manually added AudioSource
    [SerializeField] private AudioClip backgroundTrack; // Reference to the background track

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

        // Configure the AudioSource for the background track
        audioSource.clip = backgroundTrack;
        audioSource.loop = true; // Ensure looping is enabled
    }

    private void Start()
    {
        // Play background music when the game starts
        AudioManager.Instance.PlayBackgroundTrack();
    }

    public void PlayBackgroundTrack()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Play the background track
        }
    }

    public void StopBackgroundTrack()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // Stop the background track
        }
    }
}
