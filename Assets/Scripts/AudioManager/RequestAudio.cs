using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class RequestAudio : MonoBehaviour
{
    public string ClipName;
    public bool IsLooping;
    public AudioMixerGroup MixerGroup;
    [Range(0.0f,1.0f)]
    public float SpatialBlend;
    public float MinDistance3D;

    private void Start()
    {
        AudioManager.instance.PlaySound(ClipName, transform, IsLooping, MixerGroup, SpatialBlend, MinDistance3D);
    }
}
