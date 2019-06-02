public class MusicAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.MusicVolume;
        set => Audio.MusicVolume = value;
    }
}

