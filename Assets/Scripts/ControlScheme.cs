using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

[CreateAssetMenu(fileName = "NewControlScheme", menuName = "Control Scheme", order = 1)]
public class ControlScheme : ScriptableObject
{
    public KeyCode ForwardKey;
    public KeyCode BackwardKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode FireKey;

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
