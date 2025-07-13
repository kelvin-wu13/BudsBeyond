using System;
using UnityEngine;


[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 1f;

    [Range(0.1f, 3f)]
    public float pitch = 1f;

    public bool loop = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] backgroundMusic;
    public Sound[] soundEffects;

    private AudioSource bgmSource;

    void Awake()
    {
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

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;

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
        bgmSource.loop = true;
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