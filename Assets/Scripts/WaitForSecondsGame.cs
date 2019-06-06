using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class WaitForSecondsGame
{
    public static IEnumerator Wait(float Time)
    {
        while (Time > 0f)
        {
            yield return null;
            Time -= GameManager.GameDT;
        }
    }
}
