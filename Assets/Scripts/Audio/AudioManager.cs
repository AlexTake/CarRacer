using System;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public static AudioManager Instance;
    private void Awake()
    {
        Instance = this;
        foreach (Sound s in sounds)
        {
            s.source=gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.isLooped;
        }
    }

    public void Play(string audName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == audName);
        s.source.Play();
    }
}
