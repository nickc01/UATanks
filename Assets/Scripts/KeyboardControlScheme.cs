using UnityEngine;

[CreateAssetMenu(fileName = "NewKeyBoardControlScheme", menuName = "Keyboard Control Scheme", order = 1)]
public class KeyboardControlScheme : ControlScheme
{
    public KeyCode ForwardKey;
    public KeyCode BackwardKey;
    public KeyCode LeftKey;
    public KeyCode RightKey;
    public KeyCode FireKey;

    public override bool Firing => Input.GetKey(FireKey);

    public override bool MovingForward => Input.GetKey(ForwardKey);

    public override bool MovingBackward => Input.GetKey(BackwardKey);

    public override bool MovingLeft => Input.GetKey(LeftKey);

    public override bool MovingRight => Input.GetKey(RightKey);
}
