using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer Singleton;

    public static List<Transform> Listeners = new List<Transform>();

    private void Start()
    {
        //transform.position = Vector3
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    static IEnumerator AudioRoutine(AudioClip audio,float volume, Func<Vector3> Position,Func<Vector3> listener)
    {
        if (listener != null)
        {
            var audioObject = new GameObject($"Player for : {audio.name}", typeof(AudioSource));
            var audioPlayer = audioObject.GetComponent<AudioSource>();
            audioPlayer.spatialBlend = 1.0f;
            audioPlayer.spatialize = true;
            audioPlayer.dopplerLevel = 0f;
            audioPlayer.PlayOneShot(audio,volume);
            audioPlayer.rolloffMode = AudioRolloffMode.Linear;
            audioPlayer.maxDistance = 20f;
            Destroy(audioObject, audio.length);
            while (audioObject != null)
            {
                audioObject.transform.position = Position() - listener() + Singleton.transform.position;
                yield return null;
            }
        }
    }


    public static void Play(AudioClip audio,float volume,Vector3 position,Vector3 listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position,() => listenerPosition));
    }

    public static void Play(AudioClip audio, float volume, Transform position, Transform listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position != null ? position.position : new Vector3(900f,900f,900f), () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f)));
    }

    public static void Play(AudioClip audio, float volume, Vector3 position, Transform listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position, () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f)));
    }

    public static void Play(AudioClip audio, float volume, Transform position, Vector3 listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition));
    }

    public static void Play(AudioClip audio, float volume = 1)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio, volume, () => Vector3.zero, () => Vector3.zero));
    }
}
