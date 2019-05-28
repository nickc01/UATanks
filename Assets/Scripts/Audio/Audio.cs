using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public static class Audio
{
    public static float Master
    {
        get
        {
            GameManager.Game.MainAudio.GetFloat("MasterVolume", out var result);
            return Mathf.InverseLerp(-80.0f, 0f, result);
        }
        set
        {
            GameManager.Game.MainAudio.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, value));
        }
    }
    public static float Music
    {
        get
        {
            GameManager.Game.MainAudio.GetFloat("MusicVolume", out var result);
            return Mathf.InverseLerp(-80.0f, 0f, result);
        }
        set
        {
            GameManager.Game.MainAudio.SetFloat("MusicVolume", Mathf.Lerp(-80f, 0f, value));
        }
    }
    public static float SoundEffects
    {
        get
        {
            GameManager.Game.MainAudio.GetFloat("SoundEffectsVolume", out var result);
            return Mathf.InverseLerp(-80.0f, 0f, result);
        }
        set
        {
            GameManager.Game.MainAudio.SetFloat("SoundEffectsVolume", Mathf.Lerp(-80f,0f,value));
        }
    }
}
