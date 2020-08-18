
public enum Team_Type { Red, Blue, Neutral }
public enum Range_Class { Melee, Ranged }

public interface ITargetable
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
}

public interface IMove
{
    float MoveSpeed { get; }
}

public interface ITakeDamage : ICanLevel
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


