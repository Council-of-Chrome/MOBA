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
    void CastAbility(bool[] _inputs, Ray _mouseRay);
}
