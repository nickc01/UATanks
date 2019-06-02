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

    public static TMP_Dropdown MapType => Singleton.MapTypeDropdown; //The public interface for accessing the map type
    public static TMP_Dropdown PlayerCount => Singleton.PlayerCountDropdown; //The public interface for accessing the player count

    private static Options singleton;
    private static Options Singleton => singleton ?? (singleton = FindObjectOfType<Options>());

    private void Start()
    {
        //Load the stored option settings
        LoadSettings();
    }

    private void LoadSettings()
    {
        //Load the map type and player count via PlayerPref
        MapType.value = PlayerPrefs.GetInt("MapType");
        PlayerCount.value = PlayerPrefs.GetInt("PlayerCount");
    }

    private void SaveSettings()
    {
        //Save the Map Type and Player Count
        PlayerPrefs.SetInt("MapType", MapType.value);
        PlayerPrefs.SetInt("PlayerCount", PlayerCount.value);
    }

    private void OnApplicationQuit()
    {
        SaveSettings();
    }




}
