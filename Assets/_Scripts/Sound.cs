using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string Name;


    [Range(0f,1f)]
    public float Volume;
    [Range(0f, 3f)]
    public float Pitch;
    public AudioClip Clip;
    [HideInInspector]
    public AudioSource source;
    public bool Loop;
}
