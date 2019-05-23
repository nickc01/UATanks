using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
    private void OnEnable() => enabled = GameManager.CurrentCampaignLevel + 1 < GameManager.Game.Levels && GameManager.CurrentLoadMode == LevelLoadMode.Campaign;
}
