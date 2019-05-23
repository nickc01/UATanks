using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackLevelManager : MonoBehaviour
{
    private bool Loaded = false;
    public GameObject ButtonPrefab;

    private void Start()
    {
        if (!Loaded)
        {
            Loaded = true;
            for (int i = 0; i < GameManager.Game.Levels; i++)
            {
                Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CampaignLevelButton>().Level = i;
            }
        }
    }
}
