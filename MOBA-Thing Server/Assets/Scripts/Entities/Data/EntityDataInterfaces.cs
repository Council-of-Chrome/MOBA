using System;

public enum Team_Type { Blue, Neutral, Red }
public enum Range_Class { Melee, Ranged }
public enum Entity_Size { Tiny = 1, Small = 3, Average = 5, Large = 7, Huge = 9 } //when using this, divide enum value by 10 to get agent size

public interface IHasName
{
    string DisplayName { get; }
}

public interface ICanLevel
{
    int BaseLevel { get; }
    Func<int, int> MaxXPScaler { get; }
}

public interface IChangeSize
{
    Entity_Size BaseSize { get; }
}

public interface IMove
{
    float BaseMoveSpeed { get; }
}

public interface IUseHealth
{
    float BaseHP { get; }
    float HPPerLevel { get; }

    float BaseHPRegen { get; }
    float HPRegenPerLevel { get; }
}

public interface IResistDamage
{
    float BaseAura { get; }
    float AuraPerLevel { get; }
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

public interface IUseAbilities
{ 
    Ability Innate { get; }
    Ability[] Abilities { get; }
}
