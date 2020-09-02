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
    AttackDamageManager AttackDamage { get; }
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
public interface IManageSize
{
    Entity_Size BaseSize { get; }
    Entity_Size CurrentSize { get; }
}
public interface IManageConditions
{
    ConditionManager Conditions { get; }
}
public interface IManageCrowdControl
{
    CrowdControlManager CrowdControl { get; }
}
