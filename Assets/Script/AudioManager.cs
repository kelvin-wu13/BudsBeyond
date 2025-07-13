using System;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(0.1f, 3f)] public float pitch = 1f;
    public bool loop = false;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Sound[] backgroundMusic;
    public Sound[] soundEffects;
    private AudioSource bgmSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); return; }
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        PlayBGM("Main Menu");
    }

    public void PlayBGM(string name)
    {
        Sound s = Array.Find(backgroundMusic, sound => sound.name == name);
        if (s == null) { return; }
        bgmSource.clip = s.clip;
        bgmSource.volume = s.volume;
        bgmSource.pitch = s.pitch;
        bgmSource.loop = s.loop;
        bgmSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(soundEffects, sound => sound.name == name);
        if (s == null) { return; }
        sfxSource.PlayOneShot(s.clip, s.volume);
    }
}