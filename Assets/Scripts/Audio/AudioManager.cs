using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    [SerializeField]
    private AudioSource[] extSources;

    public static AudioManager instance;

    private int pVolume = 100;
    public static int volume = 100;

    private float[] sourcesInitVolume;

    private void Awake()
    {   

        // Keep this instance always loaded
        // Delete other instances
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        sourcesInitVolume = new float[extSources.Length];
        for (int i = 0; i < extSources.Length; i++)
            {
                sourcesInitVolume[i] = extSources[i].volume;
            }


        // Make audio sources with properties defined in sounds[]
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;

            s.source.pitch = s.pitch;
            

            s.source.loop = s.loop;
        }
    }

    /*
        Not exactly the best way of doing it.
        I'd use an event but when a script subscribes to an event, it subscribes to that instance of that event.
        Basically it wouldn't work after changing scenes unless the event was subscribed to again.
        That doesn't work because this script instance persists between scenes inorder for the music to keep on playing.
    */
    private void Update()
    {
        if (volume != pVolume)
        {
            UpdateVolume();
        }
        pVolume = volume;
    }

    // Update volume of all sources according to volume
    private void UpdateVolume()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume * ((float)volume / 100);
        }
        for (int i = 0; i < extSources.Length; i++)
        {
            extSources[i].volume = sourcesInitVolume[i] * ((float)volume / 100);
        }
    }
    
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
            return;
        s.source.Play();
    }
}
