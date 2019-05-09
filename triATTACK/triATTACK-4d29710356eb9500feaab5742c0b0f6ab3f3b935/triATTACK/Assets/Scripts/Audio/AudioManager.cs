﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager am;

    public Sound[] sounds;
    public static AudioManager instance;

    void Start()
    {
        if (am == null)
        {
            am = this;
        }

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Play("CountdownToStart");
        }
    }

    public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " cannot be found.");
            return;
        }

        s.source.Play();
    }

    public void Play(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogError("Sound " + name + " cannot be found.");
            return;
        }

        s.source.pitch = pitch;
        s.source.Play();
    }

}
