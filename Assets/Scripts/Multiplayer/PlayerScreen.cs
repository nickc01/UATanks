using System;

[Serializable]
public class PlayerScreen
{
    public CameraController PlayerCamera;
    public UIManager PlayerUI;



    public int PlayerNumber => PlayerUI.PlayerNumber; //The Player's Number ID
    private HealthDisplay healthInternal;
    //The Health Display for this screen
    public HealthDisplay HealthDisplay => healthInternal != null ? healthInternal : (healthInternal = PlayerUI.GetComponentInChildren<HealthDisplay>(true));
    private LivesDisplay livesInternal;
    //The Lives Display for this screen
    public LivesDisplay LivesDisplay => livesInternal != null ? livesInternal : (livesInternal = PlayerUI.GetComponentInChildren<LivesDisplay>(true));
    private ScoreDisplay scoreInternal;
    //The Score Display for this screen
    public ScoreDisplay ScoreDisplay => scoreInternal != null ? scoreInternal : (scoreInternal = PlayerUI.GetComponentInChildren<ScoreDisplay>(true));
    private HighscoreDisplay highscoreInternal;
    //The highscore Display for this screen
    public HighscoreDisplay HighscoreDisplay => highscoreInternal != null ? highscoreInternal : (highscoreInternal = PlayerUI.GetComponentInChildren<HighscoreDisplay>(true));
}
