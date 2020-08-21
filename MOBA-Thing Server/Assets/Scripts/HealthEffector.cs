using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ResourceEffector
{
    public float Value { get; private set; }
    public Stat_Effector_Type Type { get; private set; }

    public ResourceEffector(float _val, Stat_Effector_Type _type)
    {
        Value = _val;
        Type = _type;
    }
}
