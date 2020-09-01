using UnityEngine;
using UnityEngine.AI;

public class ChampionController : MonoBehaviour, IEntityTargetable, IManageHealth, IManageResource, IManageAD, IManageEXP, IManageNavAgent, IManageAbilities, IManageSize
{
    public int EntityID { get; private set; }

    public Entity_Size BaseSize => Entity_Size.Average;
    public Entity_Size CurrentSize { get { return BaseSize; } }

    public HealthManager Health { get; private set; }
    public ResourceManager Resource { get; private set; }
    public ResourceManager AttackDamage { get; private set; }
    public AbilityManager Abilities { get; private set; }
    public ExperienceManager Experience { get; private set; }
    public NavMeshAgent Agent { get; private set; }

    private CapsuleCollider hitbox;

    public void Initialize(int _entityID, ChampionData _data)
    {
        EntityID = _entityID;

        Health = new HealthManager(EntityID, _data.BaseHP, _data.HPPerLevel);
        Resource = new ResourceManager(EntityID, _data.BaseResource, _data.ResourcePerLevel);
        AttackDamage = new ResourceManager(EntityID, _data.BaseAttackDamage, _data.AttackPerLevel);
        Abilities = new AbilityManager(_entityID, _data.Abilities);

        (_data.Innate as IAbilityPassive).Init();

        Experience = new ExperienceManager(EntityID, _data.BaseLevel, _data.MaxXPScaler);
        Experience.OnLevelUp += Levelup;

        hitbox = transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();

        Agent = GetComponentInChildren<NavMeshAgent>();
        if(_data is IChangeSize)
            hitbox.radius = Agent.radius = (float)(_data as IChangeSize).BaseSize * 0.1f;
        else
            hitbox.radius = Agent.radius = (float)BaseSize * 0.1f;
        Agent.speed = _data.BaseMoveSpeed;
    }
    ~ChampionController() //safety destructor
    {
        Experience.OnLevelUp -= Levelup;
    }

    public void Levelup(int _newLevel)
    {
        Health.Levelup(_newLevel);
        Resource.Levelup(_newLevel);
        AttackDamage.Levelup(_newLevel);
        Abilities.Levelup(_newLevel);
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
    public void ChangeSize(Entity_Size _size)
    {
        Agent.radius = (float)_size * 0.1f;
    }
    #endregion
}
