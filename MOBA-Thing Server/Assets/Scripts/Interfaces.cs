
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
}

public interface IAutoAttack : ICanLevel
{
    float AttackDamage { get; }
    float AttackPerLevel { get; }

    float AttackSpeed { get; }
    float AttackSpeedPerLevel { get; }

    float AttackRange { get; }
    Range_Class RangeClass { get; }
}


