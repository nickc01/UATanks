using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

//[CreateAssetMenu(fileName = "NewControlScheme", menuName = "Control Scheme", order = 1)]
public abstract class ControlScheme : ScriptableObject
{
    public abstract bool Firing { get; } //Whether the tank should be firing or not
    public abstract bool MovingForward { get; } //Whether the tank should be moving forward or not
    public abstract bool MovingBackward { get; } //Whether the tank should be moving backward or not
    public abstract bool MovingLeft { get; } //Whether the tank should be moving left or not
    public abstract bool MovingRight { get; } //Whether the tank should be moving right or not

    //Gets the current control scheme for the player
    public static ControlScheme GetScheme(int PlayerNumber)
    {
        var field = typeof(GameManager).GetField("Player" + PlayerNumber + "Scheme");
        if (field == null || field.FieldType != typeof(ControlScheme))
        {
            throw new Exception($"There is no control scheme for Player {PlayerNumber}, add a field to the Game Manager called \"Player{PlayerNumber}Scheme\" with the ControlScheme type to give that player a control scheme");
        }
        return field.GetValue(GameManager.Game) as ControlScheme;
    }
}
