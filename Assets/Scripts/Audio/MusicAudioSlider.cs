public class MusicAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.Music;
        set => Audio.Music = value;
    }
}

