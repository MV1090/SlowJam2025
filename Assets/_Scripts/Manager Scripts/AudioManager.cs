using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public class Sound
    {
        public string name; // Name of the sound (e.g., "MoveUpSound")
        public AudioClip clip; // Audio clip to play
    }

    [SerializeField] private List<Sound> sounds = new List<Sound>(); // List of sounds
    private Dictionary<string, AudioClip> soundDictionary = new Dictionary<string, AudioClip>();
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialize AudioSource and sound dictionary
        audioSource = gameObject.AddComponent<AudioSource>();
        foreach (var sound in sounds)
        {
            soundDictionary[sound.name] = sound.clip;
        }
    }

    public void Play(string soundName)
    {
        // Play the sound if it exists in the dictionary
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found!");
        }
    }
}
