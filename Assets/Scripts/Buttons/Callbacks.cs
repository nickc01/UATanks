using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//A list of all the button callbacks in the game
public static class Callbacks
{
    //Called when the play button is pressed
    public static void Play()
    {
        GameManager.Play();
    }
}
