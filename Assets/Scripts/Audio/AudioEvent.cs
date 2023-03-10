using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioEvent", menuName = "Audio/AudioEvent", order = 1)]
public class AudioEvent : ScriptableObject
{
    public AudioClip[] AudioClip;
    public AudioMixerGroup MixerGroup;
    public bool Is2D;
    public bool Loop;
    public bool IsAttached;
    public float VolumeDecibel = 0;
    public float Pitch = 1;

    [Header("Randomness")]
    public bool RandomVolume;
    public bool RandomPitch;

    [Header("Priority")]
    public AudioManager.PriorityLevel Priority;

}
