using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Options : MonoBehaviour
{
    static Options singletonInternal;
    private static Options Singleton //The singleton for the options menu
    {
        get
        {
            if (singletonInternal == null)
            {
                singletonInternal = GameManager.Game.MainOptions;
            }
            return singletonInternal;
        }
    }

    private static EnumDropDown<MapType> mapTypeInternal;
    public static EnumDropDown<MapType> MapType //The map type dropdown
    {
        get
        {
            if (mapTypeInternal == null)
            {
                mapTypeInternal = Singleton.GetComponentInChildren<EnumDropDown<MapType>>();
            }
            return mapTypeInternal;
        }
    }

    private static EnumDropDown<PlayerCount> playerCountInternal;
    public static EnumDropDown<PlayerCount> PlayerCount //The player count dropdown
    {
        get
        {
            if (playerCountInternal == null)
            {
                playerCountInternal = Singleton.GetComponentInChildren<EnumDropDown<PlayerCount>>();
            }
            return playerCountInternal;
        }
    }

    private static EnumDropDown<Difficulty> difficultyInternal;
    public static EnumDropDown<Difficulty> Difficulty //The difficulty dropdown
    {
        get
        {
            if (difficultyInternal == null)
            {
                difficultyInternal = Singleton.GetComponentInChildren<EnumDropDown<Difficulty>>();
            }
            return difficultyInternal;
        }
    }


    private static MasterAudioSlider masterAudioInternal;
    public static MasterAudioSlider MasterAudio //The master audio slider
    {
        get
        {
            if (masterAudioInternal == null)
            {
                masterAudioInternal = Singleton.GetComponentInChildren<MasterAudioSlider>();
            }
            return masterAudioInternal;
        }
    }

    private static MusicAudioSlider musicAudioInternal;
    public static MusicAudioSlider MusicAudio //The music audio slider
    {
        get
        {
            if (musicAudioInternal == null)
            {
                musicAudioInternal = Singleton.GetComponentInChildren<MusicAudioSlider>();
            }
            return musicAudioInternal;
        }
    }

    private static SoundEffectsAudioSlider soundEffectsAudioInternal;
    public static SoundEffectsAudioSlider SoundEffectsAudio //The sound effects audio slider
    {
        get
        {
            if (soundEffectsAudioInternal == null)
            {
                soundEffectsAudioInternal = Singleton.GetComponentInChildren<SoundEffectsAudioSlider>();
            }
            return soundEffectsAudioInternal;
        }
    }

    private static MapWidthField widthFieldInternal;
    public static int MapWidth //The map width field
    {
        get
        {
            if (widthFieldInternal == null)
            {
                widthFieldInternal = Singleton.GetComponentInChildren<MapWidthField>();
            }
            return widthFieldInternal.Value;
        }
    }

    private static MapHeightField heightFieldInternal;
    public static int MapHeight //The map height field
    {
        get
        {
            if (heightFieldInternal == null)
            {
                heightFieldInternal = Singleton.GetComponentInChildren<MapHeightField>();
            }
            return heightFieldInternal.Value;
        }
    }




}
