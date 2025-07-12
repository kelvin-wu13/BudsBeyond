using System;
using UnityEngine;

// This is the helper class that defines what a "Sound" is.
// Since it's in the same file, you don't need a separate Sound.cs script.
[System.Serializable]
public class Sound
{
    public string name; // Name to identify the sound

    public AudioClip clip; // The audio file

    [Range(0f, 1f)]
    public float volume = 1f; // Volume control for this specific clip

    [Range(0.1f, 3f)]
    public float pitch = 1f; // Pitch control for this specific clip

    public bool loop = false; // Should the sound loop?

    [HideInInspector]
    public AudioSource source; // The AudioSource component that will play this sound
}

// This is the main manager class.
public class AudioManager : MonoBehaviour
{
    // Singleton instance for easy access from other scripts
    public static AudioManager instance;

    // Arrays to hold your BGM and SFX sounds
    public Sound[] backgroundMusic;
    public Sound[] soundEffects;

    // Dedicated AudioSource for BGM to prevent it from being stopped by an SFX
    public AudioSource bgmSource;

    void Awake()
    {
        // --- Singleton Pattern Setup ---
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject); // Persists across scenes

        // --- Create AudioSources for SFX ---
        // We create an AudioSource component on this GameObject for each SFX clip
        foreach (Sound s in soundEffects)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {
        // Example: Play the first BGM track when the game starts
        PlayBGM("Theme"); // Make sure you have a BGM named "Theme"
    }

    // --- Public Methods to Play Sounds ---

    /// <summary>
    /// Plays a background music track by name.
    /// </summary>
    public void PlayBGM(string name)
    {
        Sound s = Array.Find(backgroundMusic, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("BGM track not found: " + name);
            return;
        }

        bgmSource.clip = s.clip;
        bgmSource.volume = s.volume;
        bgmSource.pitch = s.pitch;
        bgmSource.loop = true; // BGM almost always loops
        bgmSource.Play();
    }

    /// <summary>
    /// Plays a sound effect by name.
    /// </summary>
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound effect not found: " + name);
            return;
        }

        // We use PlayOneShot for SFX so multiple sounds can overlap
        s.source.PlayOneShot(s.clip, s.volume);
    }
}