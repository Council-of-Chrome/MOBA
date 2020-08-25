using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityCastable : IAbilityCooldown, IAbilityCost
{
    IEnumerator Trigger(int _casterID, Vector3 _targetPos);
    TeamMask Mask { get; }
}
public interface IAbilityPassive
{
    TeamMask Mask { get; }
    void Init();
}
public interface IAbilityCooldown
{
    float[] CooldownPerLevel { get; }
}
public interface IAbilityCost
{
    float[] CostPerLevel { get; }
}