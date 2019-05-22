using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ReferenceLimitAttribute : PropertyAttribute
{
    public Type limitType;
    public ReferenceLimitAttribute(Type LimitType) => limitType = LimitType;
}