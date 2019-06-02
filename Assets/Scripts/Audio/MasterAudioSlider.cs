public class MasterAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.MasterVolume;
        set => Audio.MasterVolume = value;
    }
}

