using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class IntermittentSound : MonoBehaviour
{
    public string ClipName;
    public AudioMixerGroup MixerGroup;
    [Range(0.0f, 1.0f)]
    public float SpatialBlend;
    public float MinDistance3D;
    public float RandomTimeMax;
    public float RandomTimeMin;

    void Start()
    {
        PlayAudioAndDelay();
    }

    private void PlayAudioAndDelay()
    {
        AudioManager.instance.PlaySound(ClipName, transform, false, MixerGroup, SpatialBlend, MinDistance3D);

        float delay = Random.Range(RandomTimeMin, RandomTimeMax);
        Invoke("PlayAudioAndDelay", delay);
    }
}
