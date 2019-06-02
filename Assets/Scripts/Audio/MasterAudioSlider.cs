public class MasterAudioSlider : AudioSlider
{
    //Uses to communicate to the master slider
    //The Set property is called whenever the slider value is updated
    protected override float VolumeSetter
    {
        get => Audio.MasterVolume;
        set => Audio.MasterVolume = value;
    }
}

