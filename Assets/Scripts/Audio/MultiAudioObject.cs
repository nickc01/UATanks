using System;
using System.Collections.Generic;
using UnityEngine;

public class MultiAudioObject : AudioObject
{
    List<AudioObject> AudioObjects = new List<AudioObject>(); //The audio objects

    public override bool Done //Whether the audio objects are done or not
    {
        get => AudioObjects[0].Done;
    }

    public override bool Loop //whether the audio objects should loop or not
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

    public override Vector3 Position => AudioObjects[0].Position; //The position of the audio objects

    public override float Volume //The volume of the audio objects
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

    //Stops all the audio objects
    public override void Stop()
    {
        foreach (var obj in AudioObjects)
        {
            obj.Stop();
        }
    }

    //Creates the audio objects
    protected override void Create(AudioClip audio, Func<float> volume, Func<Vector3> Position, Func<Vector3> listener, bool loop, AudioPlayType playType)
    {
        foreach (var listenerObject in Audio.Listeners)
        {
            AudioObjects.Add(new AudioObject(audio,volume,Position,() => listenerObject == null ? Position() : listenerObject.transform.position,loop,playType));
        }
    }

    public MultiAudioObject(AudioClip audio, Func<float> volume, Func<Vector3> Position, bool loop, AudioPlayType playType) : base(audio, volume, Position, null, loop,playType) { }
}
