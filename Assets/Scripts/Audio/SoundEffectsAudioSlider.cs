public class SoundEffectsAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.SoundEffectsVolume;
        set => Audio.SoundEffectsVolume = value;
    }
}

