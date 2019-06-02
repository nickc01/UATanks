public class MusicAudioSlider : AudioSlider
{
    //Uses to communicate to the music slider
    //The Set property is called whenever the slider value is updated
    protected override float VolumeSetter
    {
        get => Audio.MusicVolume;
        set => Audio.MusicVolume = value;
    }
}

