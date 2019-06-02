using System;
using System.Collections;
using UnityEngine;


//An object that represents an audio source
public class AudioObject
{
    public virtual bool Done { get; protected set; } = false; //Whether the object is done playing audio or not

    //Whether to loop the audio or not
    private bool loop = false;
    public virtual bool Loop
    {
        get => loop;
        set => ObjectAudio.loop = loop = value;
    }

    public virtual Vector3 Position => positionFunc(); //The position of the audio object
    public virtual float Volume { get => ObjectAudio.volume; set => ObjectAudio.volume = value; } //The volume of the audio object

    private Coroutine Routine; //The audio routine
    private GameObject AudioGameObject; //The gameobject of the audio object
    private AudioSource ObjectAudio; //The audio source of the audio object

    Func<Vector3> positionFunc;

    //Creates the audio routine
    protected virtual void Create(AudioClip audio, Func<float> volume, Func<Vector3> Position, Func<Vector3> listener, bool loop,AudioPlayType playType)
    {
        Routine = CoroutineManager.StartCoroutine(AudioRoutine(audio, volume, Position, listener, loop,playType));
    }

    public AudioObject(AudioClip audio, Func<float> volume, Func<Vector3> Position, Func<Vector3> listener,bool loop, AudioPlayType playType)
    {
        //Start the audio routine
        Create(audio, volume, Position, listener, loop,playType);
    }

    IEnumerator AudioRoutine(AudioClip audio, Func<float> volume, Func<Vector3> Position, Func<Vector3> listener,bool loop, AudioPlayType playType)
    {
        if (listener != null)
        {
            float T = 0.0f;

            //Set all the stats based on the parameters passed in
            positionFunc = Position;
            AudioGameObject = new GameObject($"Audio for : {audio.name}", typeof(AudioSource));
            ObjectAudio = AudioGameObject.GetComponent<AudioSource>();
            if (playType == AudioPlayType.Spatial)
            {
                ObjectAudio.spatialBlend = 1.0f;
                ObjectAudio.spatialize = true;
            }
            else
            {
                ObjectAudio.spatialBlend = 0.0f;
                ObjectAudio.spatialize = false;
            }
            ObjectAudio.dopplerLevel = 0f;
            ObjectAudio.clip = audio;
            ObjectAudio.volume = volume();
            Loop = loop;
            //ObjectAudio.PlayOneShot(audio, volume);
            ObjectAudio.rolloffMode = AudioRolloffMode.Linear;
            ObjectAudio.maxDistance = 20f;
            ObjectAudio.Play();
            //GameObject.Destroy(audioObject, audio.length);
            while (AudioGameObject != null)
            {
                //If the audio is finished playing
                if (T >= audio.length)
                {
                    //If the audio is set to loop
                    if (Loop)
                    {
                        //Reset the time to zero and start again
                        T = 0f;
                    }
                    else
                    {
                        //Stop the audio
                        ObjectAudio.Stop();
                        //Destroy the audio object
                        GameObject.Destroy(AudioGameObject);
                        Done = true;
                        break;
                    }
                }
                //Refresh the volume
                ObjectAudio.volume = volume();
                //Refresh the object's position
                AudioGameObject.transform.position = Position() - listener() + Audio.SourceListenerPosition;
                //Wait a frame
                yield return null;
                //Increase the timer
                T += Time.deltaTime;
            }
            //If the audio source is done playing
            Done = true; //Set it to be done
            Routine = null;
        }
    }

    //Stops the audio player
    public virtual void Stop()
    {
        if (Routine != null)
        {
            //Stop the audio routine
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
