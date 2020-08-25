using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Minion", menuName = "Entities/Minions/Minion")]
public class MinionData : ScriptableObject, IUseHealth, IAutoAttack, IMove, IHasName //TODO: Build entity factory for preparing monobehaviours
{
    [SerializeField]
    private string displayName = default;
    public string DisplayName { get { return displayName; } }
    [Space]
    [SerializeField]
    private int baseLevel = 1;
    public int BaseLevel { get { return baseLevel; } }
    public Func<int, int> MaxXPScaler { get; } = (nxtlvl)
        => { return (int)(200 * Mathf.Pow(nxtlvl, 2)); };
    [Space]
    [SerializeField]
    private float moveSpeed = default;
    public float BaseMoveSpeed { get { return moveSpeed; } }
    [Space]
    [SerializeField]
    private float baseHP = default;
    public float BaseHP { get { return baseHP; } }
    [SerializeField]
    private float hpPerLevel = default;
    public float HPPerLevel { get { return hpPerLevel; } }
    [Space]
    [SerializeField]
    private float baseHPRegen = default;
    public float BaseHPRegen { get { return baseHPRegen; } }
    [SerializeField]
    private float hpRegenPerLevel = default;
    public float HPRegenPerLevel { get { return hpRegenPerLevel; } }
    [Space]
    [SerializeField]
    private float baseAttackDamage = default;
    public float BaseAttackDamage { get { return baseAttackDamage; } }
    [SerializeField]
    private float attackPerLevel = default;
    public float AttackPerLevel { get { return attackPerLevel; } }
    [Space]
    [SerializeField]
    private float baseAttackSpeed = default;
    public float BaseAttackSpeed { get { return baseAttackSpeed; } }
    [SerializeField]
    private float attackSpeedPerLevel = default;
    public float AttackSpeedPerLevel { get { return attackSpeedPerLevel; } }
    [Space]
    [SerializeField]
    private float baseAttackRange = default;
    public float BaseAttackRange { get { return baseAttackRange; } }
    [SerializeField]
    private Range_Class rangeClass = default;
    public Range_Class RangeClass { get { return rangeClass; } }
}
