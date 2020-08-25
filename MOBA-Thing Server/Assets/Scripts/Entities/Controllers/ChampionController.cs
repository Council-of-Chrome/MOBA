using UnityEngine;
using UnityEngine.AI;

public class ChampionController : MonoBehaviour, IEntityTargetable, IManageHealth, IManageResource, IManageAD, IManageEXP, IManageNavAgent, IManageAbilities
{
    public int EntityID { get; private set; }

    public HealthManager Health { get; private set; }
    public ResourceManager Resource { get; private set; }
    public ResourceManager AttackDamage { get; private set; }
    public ExperienceManager Experience { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private ChampionData data;
    private CapsuleCollider hitbox;

    public void Initialize(int _entityID, ChampionData _data)
    {
        EntityID = _entityID;

        Health = new HealthManager(EntityID, _data.BaseHP, _data.HPPerLevel);
        Resource = new ResourceManager(EntityID, _data.BaseResource, _data.ResourcePerLevel);
        AttackDamage = new ResourceManager(EntityID, _data.BaseAttackDamage, _data.AttackPerLevel);

        Experience = new ExperienceManager(EntityID, _data.BaseLevel, _data.MaxXPScaler);
        Experience.OnLevelUp += Levelup;

        hitbox = transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();

        Agent = GetComponentInChildren<NavMeshAgent>();
        /*agent.agentTypeID = Entity_Sizes.Champion*/
        Agent.speed = _data.BaseMoveSpeed;
    }
    ~ChampionController() //safety destructor
    {
        Experience.OnLevelUp -= Levelup;
    }

    public void CastAbility(bool[] _inputs, Vector3 _targetPos)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_inputs[i])
            {
                if (data.Abilities[i] is IAbilityCastable ability)
                    ability.Trigger(EntityID, _targetPos);
                return;
            }
        }
    }

    public void Levelup(int _newLevel)
    {
        Health.Levelup(_newLevel);
        Resource.Levelup(_newLevel);
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
