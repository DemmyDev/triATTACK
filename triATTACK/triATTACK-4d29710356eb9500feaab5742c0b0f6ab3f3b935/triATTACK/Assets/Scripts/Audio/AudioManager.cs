using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;

    void Start()
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

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.playOnAwake = s.playOnAwake;
            s.source.loop = s.loop;
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

        // For pitch changing on enemy hit sound
        if (s.clip.name == "EnemyHit")
        {
            s.source.pitch = FindObjectOfType<BulletCollision>().pitch;
        }

        s.source.Play();
    }

}
