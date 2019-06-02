using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyBoardControlScheme", menuName = "Keyboard Control Scheme", order = 1)]
public class KeyboardControlScheme : ControlScheme
{
    public KeyCode ForwardKey; //The key for moving forward
    public KeyCode BackwardKey; //THe key for moving backward
    public KeyCode LeftKey; //The key for moving left
    public KeyCode RightKey; //The key for moving right
    public KeyCode FireKey; //The key for firing

    public override bool Firing => Input.GetKey(FireKey); //Whether the fire key is pressed

    public override bool MovingForward => Input.GetKey(ForwardKey); //Whether the forward key is pressed

    public override bool MovingBackward => Input.GetKey(BackwardKey); //Whether the backward key is pressed

    public override bool MovingLeft => Input.GetKey(LeftKey); //Whether the left key is pressed

    public override bool MovingRight => Input.GetKey(RightKey); //Whether the right key is pressed
}
