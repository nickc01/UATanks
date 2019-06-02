public class SoundEffectsAudioSlider : AudioSlider
{
    //Uses to communicate to the sound effects slider
    //The Set property is called whenever the slider value is updated
    protected override float VolumeSetter
    {
        get => Audio.SoundEffectsVolume;
        set => Audio.SoundEffectsVolume = value;
    }
}

