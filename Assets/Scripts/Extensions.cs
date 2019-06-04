using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace UnityEngine
{
    public static class Extensions
    {
        //Converts a degrees rotation to a unit vector relative to an identity
        public static Vector3 DegToVector(float degrees, Vector3? identity = null)
        {
            return new Vector3(Mathf.Cos(RelativeDegrees(degrees, identity) * Mathf.Deg2Rad), 0f, Mathf.Sin(RelativeDegrees(degrees, identity) * Mathf.Deg2Rad));
        }

        //Returns the degrees relative to a unit vector
        public static float RelativeDegrees(float degrees, Vector3? identity = null)
        {
            identity = identity ?? Vector3.forward;
            float identityDirection = Mathf.Atan2(identity.Value.z, identity.Value.x) * Mathf.Rad2Deg;
            return identityDirection + degrees;
        }

        //Returns the number with the bigger absolute value
        public static float BiggerAbsNumber(float A, float B) => Mathf.Abs(A) > Mathf.Abs(B) ? A : B;

        //Returns the number with the lowest absolute value
        public static float LowestAbsNumber(float A, float B) => Mathf.Abs(A) < Mathf.Abs(B) ? A : B;

        //Returns the number either -1 or 1
        //If the number is less than zero, then the number is -1
        //Otherwise, it it +1
        public static float Normalize(this float number) => number >= 0f ? 1f : -1f;

        //Prints out a list. Mainly used for debug purposes
        public static void Print<T>(this List<T> list)
        {
            foreach (var item in list)
            {
                Debug.Log(item);
            }
        }

        //Returns a random element from the list
        public static T RandomElement<T>(this List<T> list)
        {
            return list[Random.Range(0,list.Count)];
        }

        //A clamp method that works with negative and positive numbers
        public static float ClampAbs(float value, float min, float max)
        {
            if (value < 0f)
            {
                return Mathf.Clamp(value, -max, -min);
            }
            else
            {
                return Mathf.Clamp(value, min, max);
            }
        }

        public static string Clean(this string value)
        {
            var result = Regex.Replace(value, @"([[A-Z\s])", @" $1");
            if (result[0] == ' ')
            {
                result = result.Remove(0, 1);
            }
            return result;
        }

        public static T GetComponentOnlyInChildren<T>(this Component parent,bool includeInactive = false) where T : Component
        {
            return parent.GetComponentsInChildren<T>(includeInactive).FirstOrDefault(c => c != parent);
        }

        public static List<T> Clone<T>(this List<T> list)
        {
            List<T> clone = new List<T>(list.Capacity);
            foreach (var item in list)
            {
                clone.Add(item);
            }
            return clone;
        }

        public static T PopRandom<T>(this List<T> list)
        {
            var item = list[Random.Range(0, list.Count)];
            list.Remove(item);
            return item;
        }

    }
}
