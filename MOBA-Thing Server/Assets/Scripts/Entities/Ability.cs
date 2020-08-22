using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    public abstract string AbilityName { get; }
    public abstract string Description { get; }
}
