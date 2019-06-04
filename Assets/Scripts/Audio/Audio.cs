using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioPlayType
{
    Spatial,
    Stereo
}

public class Audio : MonoBehaviour
{
    static bool Loaded = false; //Whether the volume meters are loaded or not
    static Audio Singleton;

    public static List<Transform> Listeners = new List<Transform>(); //A list of listeners in the game
    public static Vector3 SourceListenerPosition => Singleton.transform.position; //The position of the main listener

    //Functions for getting the volume

    public static float MasterVolume => Options.MasterAudio.Value;
    public static float MusicVolume => Mathf.Clamp(Options.MusicAudio.Value, 0f, MasterVolume);
    public static float SoundEffectsVolume => Mathf.Clamp(Options.SoundEffectsAudio.Value, 0f, MasterVolume);

    public static float Master() => MasterVolume;
    public static float Music() => MusicVolume;
    public static float SoundEffects() => SoundEffectsVolume;

    private void Start()
    {
        //Set the singleton
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

    //A series of functions used to play audio in the game

    public static AudioObject Play(AudioClip audio, Func<float> volume, Vector3 position, Vector3 listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, volume, () => position, () => listenerPosition, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, Transform position, Transform listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, Vector3 position, Transform listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, volume, () => position, () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, Transform position, Vector3 listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, volume, () => Vector3.zero, () => Vector3.zero, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, Vector3 position, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new MultiAudioObject(audio, volume, () => position, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, Func<float> volume, Transform position, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new MultiAudioObject(audio, volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Vector3 position, Vector3 listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, () => volume, () => position, () => listenerPosition, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, Transform listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, () => volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Vector3 position, Transform listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, () => volume, () => position, () => listenerPosition != null ? listenerPosition.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, Vector3 listenerPosition, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, () => volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), () => listenerPosition, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume = 1f, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new AudioObject(audio, () => volume, () => Vector3.zero, () => Vector3.zero, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Vector3 position, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new MultiAudioObject(audio, () => volume, () => position, loop, playType);
    }

    public static AudioObject Play(AudioClip audio, float volume, Transform position, bool loop = false, AudioPlayType playType = AudioPlayType.Spatial)
    {
        return new MultiAudioObject(audio, () => volume, () => position != null ? position.position : new Vector3(900f, 900f, 900f), loop, playType);
    }

}
