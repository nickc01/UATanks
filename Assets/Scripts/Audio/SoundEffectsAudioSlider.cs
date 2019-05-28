public class SoundEffectsAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.SoundEffects;
        set => Audio.SoundEffects = value;
    }
}

