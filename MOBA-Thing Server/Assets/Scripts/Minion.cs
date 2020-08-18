using UnityEngine;

public class Minion : ScriptableObject, ITakeDamage, IAutoAttack, IMove, IHasName //TODO: Build entity factory for preparing monobehaviours
{
    public string DisplayName { get; }

    public int BaseLevel { get; }

    public float BaseHP { get; }
    public float HPPerLevel { get; }

    public float MoveSpeed { get; }

    public float AttackDamage { get; }
    public float AttackPerLevel { get; }

    public float AttackSpeed { get; }
    public float AttackSpeedPerLevel { get; }

    public float AttackRange { get; }
    public Range_Class RangeClass { get; }
}
