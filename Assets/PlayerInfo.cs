using System;

[Serializable]
public class PlayerInfo
{
    public CameraController PlayerCamera;
    public UIManager PlayerUI;



    public int PlayerID => PlayerUI.PlayerNumber;
    private HealthDisplay healthInternal;
    public HealthDisplay HealthDisplay => healthInternal != null ? healthInternal : (healthInternal = PlayerUI.GetComponentInChildren<HealthDisplay>(true));
    private LivesDisplay livesInternal;
    public LivesDisplay LivesDisplay => livesInternal != null ? livesInternal : (livesInternal = PlayerUI.GetComponentInChildren<LivesDisplay>(true));
    private ScoreDisplay scoreInternal;
    public ScoreDisplay ScoreDisplay => scoreInternal != null ? scoreInternal : (scoreInternal = PlayerUI.GetComponentInChildren<ScoreDisplay>(true));
}
