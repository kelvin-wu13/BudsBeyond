using System;
using UnityEngine;

// This is the helper class that defines what a "Sound" is.
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
    public static AudioManager instance;

    public Sound[] backgroundMusic;
    public Sound[] soundEffects;

    // The script now manages this internally, so it's private.
    private AudioSource bgmSource;

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
        DontDestroyOnLoad(gameObject);

        // --- Create the BGM AudioSource automatically ---
        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false; // We control playback manually

        // --- Create AudioSources for SFX ---
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
        PlayBGM("Main Menu");
    }

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

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound effect not found: " + name);
            return;
        }
        s.source.PlayOneShot(s.clip, s.volume);
    }
}