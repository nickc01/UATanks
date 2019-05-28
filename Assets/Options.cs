using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Options : MonoBehaviour
{
    [Header("Dropdowns")]
    [Tooltip("The dropdown area for the map type")]
    public TMP_Dropdown MapTypeDropdown;
    [Tooltip("The dropdown area for the player count")]
    public TMP_Dropdown PlayerCountDropdown;

    public static TMP_Dropdown MapType => Singleton.MapTypeDropdown;
    public static TMP_Dropdown PlayerCount => Singleton.PlayerCountDropdown;

    private static Options singleton;
    private static Options Singleton => singleton ?? (singleton = FindObjectOfType<Options>());

    private void Start()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        MapType.value = PlayerPrefs.GetInt("MapType");
        PlayerCount.value = PlayerPrefs.GetInt("PlayerCount");
        Debug.Log("MASTER VOLUME = " + PlayerPrefs.GetFloat("MasterVolume"));
        Audio.Master = PlayerPrefs.GetFloat("MasterVolume");
        Audio.Music = PlayerPrefs.GetFloat("MusicVolume");
        Audio.SoundEffects = PlayerPrefs.GetFloat("SoundEffectsVolume");
    }

    private void SaveSettings()
    {
        Debug.Log("Master Volume = " + Audio.Master);
        PlayerPrefs.SetInt("MapType", MapType.value);
        PlayerPrefs.SetInt("PlayerCount", PlayerCount.value);
        PlayerPrefs.SetFloat("MasterVolume", Audio.Master);
        PlayerPrefs.SetFloat("MusicVolume", Audio.Music);
        PlayerPrefs.SetFloat("SoundEffectsVolume", Audio.SoundEffects);
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }




}
