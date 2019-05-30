using System;

[Serializable]
public class PlayerSpecifics
{
    public CameraController Camera;
    public UIManager Manager;



    public int PlayerID => Manager.PlayerID;
    private HealthDisplay healthInternal;
    public HealthDisplay Health => healthInternal != null ? healthInternal : (healthInternal = Manager.GetComponentInChildren<HealthDisplay>(true));
    private LivesDisplay livesInternal;
    public LivesDisplay Lives => livesInternal != null ? livesInternal : (livesInternal = Manager.GetComponentInChildren<LivesDisplay>(true));
    private ScoreDisplay scoreInternal;
    public ScoreDisplay Score => scoreInternal != null ? scoreInternal : (scoreInternal = Manager.GetComponentInChildren<ScoreDisplay>(true));
}
