public class MasterAudioSlider : AudioSlider
{
    protected override float VolumeSetter
    {
        get => Audio.Master;
        set => Audio.Master = value;
    }
}

