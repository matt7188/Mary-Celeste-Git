using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(string soundName, Transform target, bool isLooping, AudioMixerGroup mixerGroup, float spatialBlend, float minDistance3D)
    {
        AudioSource audioSource;

        if (target.gameObject.GetComponent<AudioSource>() && target.gameObject.GetComponent<AudioSource>().clip == null)
        {
            audioSource = target.gameObject.GetComponent<AudioSource>();
        }
        else
        {
            audioSource = target.gameObject.AddComponent<AudioSource>() as AudioSource;
        }

        foreach (Sound sound in sounds)
        {
            if (soundName == sound.name)
            {
                audioSource.clip = sound.clip;
                audioSource.volume = sound.volume;
                audioSource.pitch = sound.pitch;
                break;
            }
        }

        if (audioSource.clip == null)
        {
            throw new System.Exception("Sound " + soundName + " not found. Check input and try again.");
        }

        audioSource.loop = isLooping;
        audioSource.outputAudioMixerGroup = mixerGroup;

        if(spatialBlend <= 1.0f && spatialBlend >= 0.0f)
        {
            audioSource.spatialBlend = spatialBlend;
        }

        audioSource.minDistance = minDistance3D;

        audioSource.Play();
    }
}
