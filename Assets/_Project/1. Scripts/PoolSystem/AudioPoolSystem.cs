using System.Collections.Generic;
using UnityEngine;

public class AudioPoolSystem : MonoBehaviour
{
    #region Singleton
    public static AudioPoolSystem Singleton;
    private void Awake()
    {
        if (Singleton != null)
        {
            DestroyImmediate(Singleton);
        }

        Singleton = this;
    }
    #endregion

    public int audioPoolSize = 30;
    public Queue<AudioSource> audioqueue;
    public AudioClip[] musicClip;

    [Range(0, 1)]
    public float masterVolume = 1f;
    [Range(0, 1)]
    public float menuVolume = 1f;
    [Range(0, 1)]
    public float shootVolume = 0.3f;
    [Range(0, 1)]
    public float SFXVolume = 0.3f;
    [Range(0, 1)]
    public float musicVolume = .3f;

    private AudioSource audioSource;

    public void Initialize()
    {
        audioqueue = new Queue<AudioSource>();
        var obj = new GameObject("AudioSource");
        obj.AddComponent<AudioSource>();
        audioSource = obj.GetComponent<AudioSource>();

        for (int i = 0; i < audioPoolSize; i++)
        {
            var go = Instantiate(audioSource, transform);
            audioqueue.Enqueue(go);
        }
    }

    public AudioSource PlayAudio(AudioClip clip, float volume = .1f, bool loop = false)
    {
        AudioSource source = audioqueue.Dequeue();
        source.loop = loop;
        source.spatialBlend = 0;
        source.volume = volume * masterVolume;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);

        return source;
    }


    public void PlayShootAudio(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.spatialBlend = 0;
        source.volume = (volume * shootVolume * masterVolume) * .05f;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayAudioSFX(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.spatialBlend = 0;
        source.volume = (volume * SFXVolume * masterVolume) * .05f;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayAudioMenu(AudioClip clip, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();
        source.spatialBlend = 0;
        source.volume = volume * menuVolume * masterVolume * 0.01f;
        source.clip = clip;
        source.Play();

        audioqueue.Enqueue(source);
    }

    public void PlayAudioAtLocation(AudioClip clip, Vector3 position, float volume = .1f)
    {
        AudioSource source = audioqueue.Dequeue();

        source.spatialBlend = 1;
        source.volume = volume * SFXVolume * masterVolume;
        source.clip = clip;
        source.transform.position = position;
        source.Play();

        audioqueue.Enqueue(source);
    } 
} 

