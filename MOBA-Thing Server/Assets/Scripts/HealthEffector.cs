using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ResourceEffector
{
    public float Value;
    public Stat_Effector_Type Type;

    public ResourceEffector(float _val, Stat_Effector_Type _type)
    {
        Value = _val;
        Type = _type;
    }
}
