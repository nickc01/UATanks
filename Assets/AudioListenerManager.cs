using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioListenerManager : MonoBehaviour
{
    static AudioListenerManager Singleton;

    private void Start()
    {
        transform.position = new Vector3(800f,800f,800f);
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
            audioPlayer.PlayOneShot(audio,volume);
            Destroy(audioObject, audio.length);
            while (audioObject != null)
            {
                audioObject.transform.position = listener() - Position() + Singleton.transform.position;
                yield return null;
            }
        }
    }


    public static void PlayAudio(AudioClip audio,float volume,Vector3 position,Vector3 listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position,() => listenerPosition));
    }

    public static void PlayAudio(AudioClip audio, float volume, Transform position, Transform listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position.transform.position, () => listenerPosition.transform.position));
    }

    public static void PlayAudio(AudioClip audio, float volume, Vector3 position, Transform listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position, () => listenerPosition.transform.position));
    }

    public static void PlayAudio(AudioClip audio, float volume, Transform position, Vector3 listenerPosition)
    {
        CoroutineManager.StartCoroutine(AudioRoutine(audio,volume, () => position.transform.position, () => listenerPosition));
    }
}
