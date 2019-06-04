using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class MinimapPrefab : MonoBehaviour
{
    [HideInInspector]
    public Transform Source;

    private Tank sourceTank;

    private void Start()
    {
        sourceTank = Source.GetComponent<Tank>();
        if (sourceTank is EnemyTank)
        {
            GetComponent<Image>().color = sourceTank.Data.TankColor;
        }
    }
}
