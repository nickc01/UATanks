using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine
{
    public static class Extensions
    {
        //Converts a degrees rotation to a unit vector relative to an identity
        public static Vector3 DegToVector(float degrees, Vector3? identity = null)
        {
            //identity = identity ?? Vector3.forward;
            //float identityDirection = Mathf.Atan2(identity.Value.z, identity.Value.x);
            return new Vector3(Mathf.Cos(RelativeDegrees(degrees, identity) * Mathf.Deg2Rad), 0f, Mathf.Sin(RelativeDegrees(degrees, identity) * Mathf.Deg2Rad));
        }

        //Returns the degrees relative to a unit vector
        public static float RelativeDegrees(float degrees, Vector3? identity = null)
        {
            identity = identity ?? Vector3.forward;
            float identityDirection = Mathf.Atan2(identity.Value.z, identity.Value.x) * Mathf.Rad2Deg;
            return identityDirection + degrees;
        }

        public static float MostExtreme(float A, float B) => Mathf.Abs(A) > Mathf.Abs(B) ? A : B;
        public static float LeastExtreme(float A, float B) => Mathf.Abs(A) < Mathf.Abs(B) ? A : B;

        public static float ToSignOnly(this float number) => number >= 0f ? 1f : -1f;

        public static void Print<T>(this List<T> list)
        {
            foreach (var item in list)
            {
                Debug.Log(item);
            }
        }
    }
}
