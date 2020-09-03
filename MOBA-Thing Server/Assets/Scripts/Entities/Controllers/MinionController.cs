using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinionController : MonoBehaviour, IEntityTargetable, IManageHealth, IManageAD, IManageNavAgent, IGrantVision
{
    public int EntityID { get; private set; }

    public int BaseVisionRadius => 11;
    public int CurrentVisionRadius { get; private set; } = 11;

    public HealthManager Health { get; private set; }
    public AttackDamageManager AttackDamage { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private MinionData data;
    private CapsuleCollider hitbox;

    public void Initialize(int _entityID, MinionData _data)
    {
        EntityID = _entityID;

        Health = new HealthManager(EntityID, _data.BaseHP, _data.HPPerLevel, 23f);
        AttackDamage = new AttackDamageManager(EntityID, _data.BaseAttackDamage, _data.AttackPerLevel);

        hitbox = transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();

        Agent = GetComponentInChildren<NavMeshAgent>();
        /*agent.agentTypeID = Entity_Sizes.Minion*/
        Agent.speed = _data.BaseMoveSpeed;
    }

    public void Levelup(int _newLevel)
    {
        Health.Levelup(_newLevel);
        AttackDamage.Levelup(_newLevel);
    }

    #region Helpers
    public Vector3 GetPosition()
    {
        return Agent.transform.position;
    }
    public void MoveTo(Vector3 _target)
    {
        Agent.SetDestination(_target);
    }
    #endregion
}
