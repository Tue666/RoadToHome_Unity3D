using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MusicSound
{
    public string name;
    public AudioClip clip;
    [Range(0, 256)]
    public int priority = 128;
    [Range(0f, 1f)]
    public float volume = 1f;
    [Range(-3f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
}

[Serializable]
public class EffectSound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public MusicSound[] musicSounds;

    public EffectSound[] weaponSounds;
    public EffectSound[] playerSounds;
    public EffectSound[] enemySounds;
    private Dictionary<string, AudioClip> weaponSoundsDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> playerSoundsDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> enemySoundsDictionary = new Dictionary<string, AudioClip>();

    public AudioSource effectSource;
    public AudioSource musicSource;

    void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
        InitDictionary();
    }

    void InitDictionary()
    {
        foreach (EffectSound sound in weaponSounds)
        {
            if (sound != null)
                weaponSoundsDictionary.Add(sound.name, sound.clip);
        }
        foreach (EffectSound sound in playerSounds)
        {
            if (sound != null)
                playerSoundsDictionary.Add(sound.name, sound.clip);
        }
        foreach (EffectSound sound in enemySounds)
        {
            if (sound != null)
                enemySoundsDictionary.Add(sound.name, sound.clip);
        }
    }

    public void PlayMusic(string soundName)
    {
        MusicSound sound = Array.Find(musicSounds, sound => sound.name == soundName);
        if (sound == null) return;
        musicSource.clip = sound.clip;
        musicSource.priority = sound.priority;
        musicSource.volume = sound.volume;
        musicSource.pitch = sound.pitch;
        musicSource.loop = sound.loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
            musicSource.Stop();
    }

    public void PlayEffect(string type, string soundName, bool stopSoundsBefore = false)
    {
        AudioClip clip = null;
        switch (type)
        {
            case "WEAPON":
                if (Helpers.ContainsKeyButValueNotNull(weaponSoundsDictionary, soundName))
                    clip = weaponSoundsDictionary[soundName];
                break;
            case "PLAYER":
                if (Helpers.ContainsKeyButValueNotNull(playerSoundsDictionary, soundName))
                    clip = playerSoundsDictionary[soundName];
                break;
            case "ENEMY":
                if (Helpers.ContainsKeyButValueNotNull(enemySoundsDictionary, soundName))
                    clip = enemySoundsDictionary[soundName];
                break;
            default:
                break;
        }
        if (clip == null) return;
        if (stopSoundsBefore && effectSource.isPlaying) effectSource.Stop();
        effectSource.PlayOneShot(clip);
    }
}
