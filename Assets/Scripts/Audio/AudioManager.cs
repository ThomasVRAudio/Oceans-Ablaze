using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMathFunctions;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource[] _lowPrioritySources;
    [SerializeField] private AudioSource[] _highPrioritySources;

    [Header("Audio Events")]
    public AudioEvent SFX_CannonFire;
    public AudioEvent SFX_Ambience;
    public AudioEvent SFX_WoodCreak;
    public enum PriorityLevel { Low, High };

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Play(AudioEvent audioEvent, Vector3 position, Transform trans = null)
    {
        AudioSource source = GetSource(audioEvent.Priority);
        if (source == null)
            return;

        SetupSource(source.GetComponent<AudioSource>(), audioEvent);

        source.transform.position = position;
        source.transform.parent = audioEvent.IsAttached && trans != null ? trans : transform;

        source.GetComponent<AudioSource>().Play();
    }

    AudioSource GetSource(PriorityLevel priority)
    {
        AudioSource[] sources = priority == PriorityLevel.Low ? _lowPrioritySources : _highPrioritySources;
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
                return source;
        }

        return null;
    }

    public void SetupSource(AudioSource source, AudioEvent audioEvent)
    {
        float volumeModifier = audioEvent.RandomVolume ? Random.Range(0.8f, 1.2f): 1;
        source.volume = AMathfs.DecibelToLinear(audioEvent.VolumeDecibel) * volumeModifier;
        float pitchModifier = audioEvent.RandomPitch ? Random.Range(0.8f, 1.2f) : 1;
        source.pitch = audioEvent.Pitch * pitchModifier;

        source.loop = audioEvent.Loop;
        source.spatialBlend = audioEvent.Is2D ? 0 : 1;

        source.outputAudioMixerGroup = audioEvent.MixerGroup;

        if (audioEvent.AudioClip.Length > 1)
            source.clip = audioEvent.AudioClip[Random.Range(0, audioEvent.AudioClip.Length)];
        else
            source.clip = audioEvent.AudioClip[0];
    }
}
