using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilityCastable : IAbilityCooldown, IAbilityCost
{
    Timer CooldownTimer { get; }
    float CastTime { get; }
    void Trigger(int _casterID, Ray _mouseRay, int _abilityRank);
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