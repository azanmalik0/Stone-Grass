using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.Clip;
            s.source.volume = s.Volume;
            s.source.pitch = s.Pitch;
            s.source.loop = s.Loop;
        }

    }

    public void Play(string name)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            Sound s = Array.Find(sounds, sound => sound.Name == name);
            if (s == null)
            {
                return;
            }
            s.source.Play();
        }

    }
    public void Play(string name, float delay)
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            Sound s = Array.Find(sounds, sound => sound.Name == name);
            if (s == null)
            {
                return;
            }
            s.source.PlayDelayed(delay);
        }

    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null)
        {
            return;
        }
        s.source.Stop();

    }
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null)
        {
            return;
        }
        s.source.Pause();

    }
    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.Name == name);
        if (s == null)
        {
            return false;
        }
        return s.source.isPlaying;

    }
}
