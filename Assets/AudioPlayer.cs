using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioObject
{
    public virtual bool Done { get; protected set; } = false;

    private bool loop = false;
    public virtual bool Loop
    {
        get => loop;
        set => ObjectAudio.loop = loop = value;
    }

    public virtual Vector3 Position => positionFunc();
    public virtual float Volume { get => ObjectAudio.volume; set => ObjectAudio.volume = value; }

    private Coroutine Routine;
    private GameObject AudioGameObject;
    private AudioSource ObjectAudio;

    Func<Vector3> positionFunc;

    protected virtual void Create(AudioClip audio, float volume, Func<Vector3> Position, Func<Vector3> listener, bool loop)
    {
        Routine = CoroutineManager.StartCoroutine(AudioRoutine(audio, volume, Position, listener, loop));
    }

    public AudioObject(AudioClip audio, float volume, Func<Vector3> Position, Func<Vector3> listener,bool loop)
    {
        Create(audio, volume, Position, listener, loop);
    }

    IEnumerator AudioRoutine(AudioClip audio, float volume, Func<Vector3> Position, Func<Vector3> listener,bool loop)
    {
        if (listener != null)
        {
            float T = 0.0f;

            positionFunc = Position;
            AudioGameObject = new GameObject($"Audio for : {audio.name}", typeof(AudioSource));
            ObjectAudio = AudioGameObject.GetComponent<AudioSource>();
            ObjectAudio.spatialBlend = 1.0f;
            ObjectAudio.spatialize = true;
            ObjectAudio.dopplerLevel = 0f;
            ObjectAudio.clip = audio;
            ObjectAudio.volume = volume;
            Loop = loop;
            //ObjectAudio.PlayOneShot(audio, volume);
            ObjectAudio.rolloffMode = AudioRolloffMode.Linear;
            ObjectAudio.maxDistance = 20f;
            ObjectAudio.Play();
            //GameObject.Destroy(audioObject, audio.length);
            while (AudioGameObject != null)
            {
                if (T >= audio.length)
                {
                    if (Loop)
                    {
                        T = 0f;
                    }
                    else
                    {
                        ObjectAudio.Stop();
                        GameObject.Destroy(AudioGameObject);
                        break;
                    }
                }
                AudioGameObject.transform.position = Position() - listener() + AudioPlayer.PlayerPosition;
                yield return null;
                T += Time.deltaTime;
            }
            Done = true;
            Routine = null;
        }
    }

    public virtual void Stop()
    {
        if (Routine != null)
        {
            CoroutineManager.StopCoroutine(Routine);
            if (AudioGameObject != null)
            {
                ObjectAudio.Stop();
                GameObject.Destroy(AudioGameObject);
            }
        }
        Done = true;
    }
}

public class MultiAudioObject : AudioObject
{
    List<AudioObject> AudioObjects = new List<AudioObject>();

    public override bool Done
    {
        get => AudioObjects[0].Done;
    }

    public override bool Loop
    {
        get => AudioObjects[0].Loop;
        set
        {
            foreach (var obj in AudioObjects)
            {
                obj.Loop = value;
            }
        }
    }

    public override Vector3 Position => AudioObjects[0].Position;

    public override float Volume
    {
        get => AudioObjects[0].Volume;
        set
        {
            foreach (var obj in AudioObjects)
            {
                obj.Volume = value;
            }
        }
    }

    public override void Stop()
    {
        foreach (var obj in AudioObjects)
        {
            obj.Stop();
        }
    }

    protected override void Create(AudioClip audio, float volume, Func<Vector3> Position, Func<Vector3> listener, bool loop)
    {
        foreach (var listenerObject in AudioPlayer.Listeners)
        {
            AudioObjects.Add(new AudioObject(audio,volume,Position,() => listenerObject == null ? Position() : listenerObject.transform.position,loop));
        }
    }

    public MultiAudioObject(AudioClip audio, float volume, Func<Vector3> Position, bool loop) : base(audio, volume, Position, null, loop) { }
}

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer Singleton;

    public static List<Transform> Listeners = new List<Transform>();

    public static Vector3 PlayerPosition => Singleton.transform.position;

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


    public static AudioObject Play(AudioClip audio,float volume,Vector3 position,Vector3 listenerPosition,bool loop = false)
    {
        return new AudioObject(audio,volume, () => position,() => listenerPosition,loop);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, Transform listenerPosition,bool loop = false)
    {
        return new AudioObject(audio,volume, () => position != null ? position.position : new Vector3(900f,900f,900f), () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f),loop);
    }

    public static AudioObject Play(AudioClip audio, float volume, Vector3 position, Transform listenerPosition,bool loop = false)
    {
        return new AudioObject(audio,volume, () => position, () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f),loop);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, Vector3 listenerPosition,bool loop = false)
    {
        return new AudioObject(audio,volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition,loop);
    }

    public static AudioObject Play(AudioClip audio, float volume = 1,bool loop = false)
    {
        //return new MultiAudioObject(audio, volume, () => Vector3.zero, loop);
        return new AudioObject(audio, volume, () => Vector3.zero, () => Vector3.zero, loop);
    }

    public static AudioObject Play(AudioClip audio, float volume, Vector3 position, bool loop = false)
    {
        return new MultiAudioObject(audio, volume, () => position, loop);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, bool loop = false)
    {
        return new MultiAudioObject(audio, volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), loop);
    }
}
