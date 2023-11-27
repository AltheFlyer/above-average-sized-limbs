using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXManager : Singleton<SFXManager>
{
    private static SFXList sfxListBuffer;

    public static float sfxVolume = 1;
    private static float sfxVolumeFac = 0.5f;

    public SFXList sfxList;
    private List<AudioSource> audioSources;

    public override void Awake()
    {
        base.Awake();

        audioSources = new List<AudioSource>();

        if (sfxList != null)
        {
            sfxListBuffer = sfxList;
        }
    }

    public void Start()
    {
        SetSFXVolume(sfxVolume);
    }

    public void PlaySFX(string name, GameObject target)
    {
        if (!Array.Exists(sfxList.sounds, sound => sound.name == name))
        {
            Debug.LogError($"The sound corresponding to {name} doesn't exist!");
            return;
        }

        // find sound
        SFXSound s = Array.Find(sfxList.sounds, sound => sound.name == name);

        // if can't find sound with the same name
        // Note: this won't actually work, since Array.Find returns the 
        // default value if none is found, which is a problem since it's 
        // effectively a Null value that isn't actually equal to null.
        if (s == null)
        {
            // if 
            if (name.Equals("")) return;
            Debug.Log("SFXManager");
        }

        // ensuring source
        AudioSource source = target.GetComponent<AudioSource>();
        if (source == null)
        {
            source = target.AddComponent<AudioSource>();
            source.volume = sfxVolume * sfxVolumeFac;
            if (s.audioMixerGroup != null)
                source.outputAudioMixerGroup = s.audioMixerGroup;
            audioSources.Add(source);

            source.maxDistance = 30f;
            source.minDistance = 0f;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.spatialBlend = 1f;
        }

        if (source.enabled) source.PlayOneShot(s.clip, s.volume);
    }

    public void SetSFXVolume(float vol)
    {
        sfxVolume = vol;
        foreach (AudioSource a in audioSources)
        {
            if (a == null)
            {
                audioSources.Remove(a);
                continue;
            }
            else
            {
                a.volume = sfxVolume * sfxVolumeFac;
            }
        }
    }

    public static void TryPlaySFX(string name, GameObject target)
    {
        if (instance == null)
        {
            Debug.Log("SFXMANAGER IS NULL, please add one into the scene from prefab folder");
            return;
        }

        instance.PlaySFX(name, target);
    }

    public static void TryPlaySFX(string[] names, GameObject target)
    {
        if (instance == null)
        {
            Debug.Log("SFXMANAGER IS NULL, please add one into the scene from prefab folder");
            return;
        }

        int i = UnityEngine.Random.Range(0, names.Length);
        instance.PlaySFX(names[i], target);
    }
}
