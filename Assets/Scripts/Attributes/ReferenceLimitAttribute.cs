using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//Limits an Object field to use a select type
public class ReferenceLimitAttribute : PropertyAttribute
{
    public Type limitType;
    public ReferenceLimitAttribute(Type LimitType) => limitType = LimitType;
}