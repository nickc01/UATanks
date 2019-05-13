using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Extensions;

//Contains helper functions to help draw shapes in the scene
public static class DebugDraw
{
    //A helper function used to draw a debug circle
    public static void DrawCircle2D(Vector3 origin, float Radius, int Subsections = 100, Color? color = null, Func<float,float,(float Angle,float Radius)> modifier = null)
    {
        color = color ?? Color.white;
        var angleDifference = 360f / Subsections;
        for (int i = 0; i < Subsections; i++)
        {
            var FirstAngle = angleDifference * i;
            var SecondAngle = angleDifference * (i + 1);
            var FirstRadius = Radius;
            var SecondRadius = Radius;
            if (modifier != null)
            {
                (FirstAngle, FirstRadius) = modifier(FirstAngle, FirstRadius);
                (SecondAngle, SecondRadius) = modifier(SecondAngle, SecondRadius);
            }

            Vector3 start = new Vector3(origin.x,origin.y, origin.z) + new Vector3(Mathf.Cos(FirstAngle * Mathf.Deg2Rad) * FirstRadius,origin.y, Mathf.Sin(FirstAngle * Mathf.Deg2Rad) * FirstRadius);
            Vector3 end = new Vector3(origin.x,origin.y, origin.z) + new Vector3(Mathf.Cos(SecondAngle * Mathf.Deg2Rad) * SecondRadius,origin.y, Mathf.Sin(SecondAngle * Mathf.Deg2Rad) * SecondRadius);
            Debug.DrawLine(start, end, color.Value);
        }
    }

    //A helper function for visualizing a FOV
    public static void DrawFOV(Vector3 origin,float Radius, float Direction,float FOV,Color? color = null)
    {
        color = color ?? Color.white;
        
        Vector3 firstEnd = origin + new Vector3(Mathf.Cos(RelativeDegrees(Direction - (FOV / 2)) * Mathf.Deg2Rad) * Radius,origin.y, Mathf.Sin(RelativeDegrees(Direction - (FOV / 2)) * Mathf.Deg2Rad) * Radius);
        Vector3 secondEnd = origin + new Vector3(Mathf.Cos(RelativeDegrees(Direction + (FOV / 2)) * Mathf.Deg2Rad) * Radius, origin.y, Mathf.Sin(RelativeDegrees(Direction + (FOV / 2)) * Mathf.Deg2Rad) * Radius);
        Debug.DrawLine(origin, firstEnd,color.Value);
        Debug.DrawLine(origin, secondEnd,color.Value);
    }
}
