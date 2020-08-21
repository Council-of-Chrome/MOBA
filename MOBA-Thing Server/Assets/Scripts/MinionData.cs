using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Entities/Minions/Minion")]
public class MinionData : ScriptableObject, IUseHealth, IAutoAttack, IMove, IHasName //TODO: Build entity factory for preparing monobehaviours
{
    public string DisplayName { get; }

    public int BaseLevel { get; }

    public float BaseHP { get; }
    public float HPPerLevel { get; }

    public float BaseHPRegen { get; }
    public float HPRegenPerLevel { get; }

    public float MoveSpeed { get; }

    public float BaseAttackDamage { get; }
    public float AttackPerLevel { get; }

    public float BaseAttackSpeed { get; }
    public float AttackSpeedPerLevel { get; }

    public float BaseAttackRange { get; }
    public Range_Class RangeClass { get; }

    public Func<int, int> MaxXPScaler { get; } = (nxtlvl) 
        => { return (int)(200 * Mathf.Pow(nxtlvl, 2)); };
}
