using TMPro;

public class HighscoreDisplay : Display<float>
{
    TextMeshProUGUI Text; //The text object that represents the highscore

    public override float Value //The value of the highscore display
    {
        get => base.Value;
        set
        {
            if (Text == null)
            {
                Text = GetComponent<TextMeshProUGUI>();
            }
            Text.text = (base.Value = value).ToString();
        }
    }

    protected override void Start()
    {
        Text = GetComponent<TextMeshProUGUI>();
    }
}

