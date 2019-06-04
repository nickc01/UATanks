using System;
using UnityEngine;

public static class Curves
{
    public static AnimationCurve Smooth => UIManager.Primary.SmoothCurve; //Gives easy access to the smooth curve
    public static AnimationCurve ReadyCurve => UIManager.Primary.ReadyScreenCurve; //Gives easy access to the ready screen curve
    public static Func<float, float> Smoother => v => Smooth.Evaluate(Smooth.Evaluate(v));
    public static Func<float, float> Smoothest => v => Smoother(Smooth.Evaluate(v));
}
