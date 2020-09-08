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

public enum Damage_Type { Physical, Magical, True }

public struct HealthEffector
{
    public float Value;
    public Damage_Type Type;
    public Stat_Effector_Type StatType;

    public HealthEffector(float _val, Damage_Type _type, Stat_Effector_Type _statType)
    {
        Value = _val;
        Type = _type;
        StatType = _statType;
    }
}
