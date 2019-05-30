using UnityEngine;

public static class Curves
{
    public static AnimationCurve Smooth => UIManager.Primary.SmoothCurve; //Gives easy access to the smooth curve
    public static AnimationCurve ReadyCurve => UIManager.Primary.ReadyScreenCurve; //Gives easy access to the ready screen curve
}
