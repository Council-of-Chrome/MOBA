
using System;

public enum Team_Type { Red, Blue, Neutral }
public enum Range_Class { Melee, Ranged }

//used for right click interactions in the game world
public interface IEntityTargetable
{
    int EntityID { get; }
}

public interface IHasName
{
    string DisplayName { get; }
}

public interface ICanLevel
{
    int BaseLevel { get; }
    Func<int, int> MaxXPScaler { get; }
}

public interface IMove
{
    float MoveSpeed { get; }
}

public interface IUseHealth
{
    float BaseHP { get; }
    float HPPerLevel { get; }

    float BaseHPRegen { get; }
    float HPRegenPerLevel { get; }
}

public interface IAutoAttack
{
    float BaseAttackDamage { get; }
    float AttackPerLevel { get; }

    float BaseAttackSpeed { get; }
    float AttackSpeedPerLevel { get; }

    float BaseAttackRange { get; }
    Range_Class RangeClass { get; }
}

public interface IConsumeResource
{
    float BaseResource { get; }
    float ResourcePerLevel { get; }

    float BaseResourceRegen { get; }
    float ResourceRegenPerLevel { get; }
}

/*public interface ICastAbility
 * {
 *  AbilityData[] Abilities { get; }
 * }
 */