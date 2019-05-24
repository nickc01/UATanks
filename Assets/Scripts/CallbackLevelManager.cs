using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackLevelManager : MonoBehaviour
{
    private bool Loaded = false;
    public GameObject ButtonPrefab; //The prefab used for each of the buttons

    private void Start()
    {
        if (!Loaded)
        {
            Loaded = true;
            //Spawn all the buttons used for the campaign
            for (int i = 0; i < GameManager.Game.Levels; i++)
            {
                Instantiate(ButtonPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<CampaignLevelButton>().Level = i;
            }
        }
    }
}
