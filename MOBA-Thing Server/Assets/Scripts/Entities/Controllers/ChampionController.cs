using UnityEngine;
using UnityEngine.AI;

public class ChampionController : MonoBehaviour, IEntityTargetable, IManageHealth, IManageResource, IManageAD, IManageEXP, IManageNavAgent, IManageAbilities, IManageConditions, IManageSize, IManageCrowdControl, IManageCombatState, IManageAuras, IGrantVision
{
    public int EntityID { get; private set; }

    public float PhysicalDamageTaken { get; private set; }
    public float MagicalDamageTaken { get; private set; }

    public bool InCombat { get; private set; } = false;
    public Timer CombatTimer { get; private set; }

    public Entity_Size BaseSize => Entity_Size.Average;
    public Entity_Size CurrentSize { get { return BaseSize; } } //not dynamically updating yet

    public int BaseVisionRadius => 50; //24 seems pretty good, maybe slightly larger, 50 for testing
    public int CurrentVisionRadius { get; private set; } = 50;

    public HealthManager Health { get; private set; }
    public AuraManager Auras { get; private set; }
    public ResourceManager Resource { get; private set; }
    public AttackDamageManager AttackDamage { get; private set; }
    public AbilityManager Abilities { get; private set; }
    public ConditionManager Conditions { get; private set; }
    public CrowdControlManager CrowdControl { get; private set; }
    public ExperienceManager Experience { get; private set; }

    public NavMeshAgent Agent { get; private set; }

    private CapsuleCollider hitbox;

    public void Initialize(int _entityID, ChampionData _data)
    {
        EntityID = _entityID;

        Auras = new AuraManager(EntityID, _data.BaseAura, _data.AuraPerLevel);

        CombatTimer = new Timer(
            () => { InCombat = true; }, 
            null, 
            () => { InCombat = false; Auras.Update(PhysicalDamageTaken, MagicalDamageTaken); }
            );

        Health = new HealthManager(EntityID, _data.BaseHP, _data.HPPerLevel);
        Health.OnHealthModified += StartCombat;

        Resource = new ResourceManager(EntityID, _data.BaseResource, _data.ResourcePerLevel);
        AttackDamage = new AttackDamageManager(EntityID, _data.BaseAttackDamage, _data.AttackPerLevel);
        Abilities = new AbilityManager(EntityID, _data.Abilities);
        Conditions = new ConditionManager(EntityID);
        CrowdControl = new CrowdControlManager(EntityID);

        if(_data.Innate is IAbilityPassive)
            (_data.Innate as IAbilityPassive).Init();

        Experience = new ExperienceManager(EntityID, _data.BaseLevel, _data.MaxXPScaler);
        Experience.OnLevelUp += Levelup;

        hitbox = transform.GetChild(0).GetComponentInChildren<CapsuleCollider>();

        Agent = GetComponentInChildren<NavMeshAgent>();
        if(_data is IChangeSize)
            hitbox.radius = Agent.radius = ((int)(_data as IChangeSize).BaseSize) * 0.1f;
        else
            hitbox.radius = Agent.radius = ((int)BaseSize) * 0.1f;
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
        Auras.Levelup(_newLevel);
    }

    public void StartCombat(HPModifiedInfo _info)
    {
        if (_info.AttackerID == EntityID) //can't fight urself 4hed
            return;

        float val = (_info.NewHP - _info.OldHP);
        if (Mathf.Sign(val) == -1)
        {
            switch (_info.DamageType)
            {
                case Damage_Type.Magical:
                    MagicalDamageTaken += Mathf.Abs(val);
                    break;
                case Damage_Type.Physical:
                    PhysicalDamageTaken += Mathf.Abs(val);
                    break;
            }
        }

        if(CombatTimer.Active)
        {
            CombatTimer.AbruptReset();
            return;
        }
        CombatTimer.Start(5f); //currently "in combat duration" is just hard coded as 5f
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
        hitbox.radius = Agent.radius = ((int)_size) * 0.1f;
    }
    #endregion
}
