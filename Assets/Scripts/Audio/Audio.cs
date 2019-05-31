using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

public static class Audio
{
    static bool Loaded = false;

    public static float Master
    {
        get
        {
            Load();
            GameManager.Game.MainAudio.GetFloat("MasterVolume", out var result);
            return Mathf.InverseLerp(-80.0f, 0f, result);
        }
        set
        {
            PlayerPrefs.SetFloat("MasterVolume", value);
            GameManager.Game.MainAudio.SetFloat("MasterVolume", Mathf.Lerp(-80f, 0f, value));
        }
    }
    public static float Music
    {
        get
        {
            Load();
            GameManager.Game.MainAudio.GetFloat("MusicVolume", out var result);
            return Mathf.InverseLerp(-80.0f, 0f, result);
        }
        set
        {
            Load();
            PlayerPrefs.SetFloat("MusicVolume", value);
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
            PlayerPrefs.SetFloat("SoundEffectsVolume", value);
            GameManager.Game.MainAudio.SetFloat("SoundEffectsVolume", Mathf.Lerp(-80f,0f,value));
        }
    }

    private static void Load()
    {
        if (Loaded)
        {
            return;
        }
        Loaded = true;
        Master = PlayerPrefs.GetFloat("MasterVolume");
        Music = PlayerPrefs.GetFloat("MusicVolume");
        SoundEffects = PlayerPrefs.GetFloat("SoundEffectsVolume");
    }
}
