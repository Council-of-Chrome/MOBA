using UnityEngine;
using UnityEngine.AI;

//used for right click interactions in the game world
public interface IEntityTargetable
{
    int EntityID { get; }
    Vector3 GetPosition();
}
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
public interface IManageCombatState
{
    float PhysicalDamageTaken { get; }
    float MagicalDamageTaken { get; }
    bool InCombat { get; }
    Timer CombatTimer { get; }
    void StartCombat(HPModifiedInfo _info);
}
public interface IManageAuras
{
    float PhysicalDamageTaken { get; }
    float MagicalDamageTaken { get; }
    AuraManager Auras { get; }
}
public interface IGrantVision
{
    int BaseVisionRadius { get; }
    int CurrentVisionRadius { get; }
}
