using UnityEngine;
using UnityEngine.AI;

public interface IManageHealth
{
    HealthManager Health { get; }
}
public interface IManageResource
{
    ResourceManager Resource { get; }
}
public interface IManageAD
{
    ResourceManager AttackDamage { get; }
}
public interface IManageEXP
{
    ExperienceManager Experience { get; }
    void Levelup(int _newLevel);
}
public interface IManageNavAgent
{
    NavMeshAgent Agent { get; }
    Vector3 GetPosition();
    void MoveTo(Vector3 _target);
}
public interface IManageAbilities
{
    AbilityManager Abilities { get; }
}
//when using this, divide enum value by 10 to get agent size
public enum Entity_Size { Tiny = 1, Small = 3, Average = 5, Large = 7, Huge = 9 } 
public interface IManageSize
{
    Entity_Size BaseSize { get; }
    Entity_Size CurrentSize { get; }
}
public interface IManageConditions
{
    ConditionManager Conditions { get; }
}
