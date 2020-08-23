using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ChampionController : MonoBehaviour, IEntityTargetable
{
    public int EntityID { get; private set; }

    public HealthManager Health { get; private set; }
    public ResourceManager Resource { get; private set; }
    public ResourceManager AttackDamage { get; private set; }
    public ExperienceManager Experience { get; private set; }

    private NavMeshAgent agent;
    private ChampionData data;

    public void Initialize(int _entityID, ChampionData _data)
    {
        EntityID = _entityID;

        Health = new HealthManager(EntityID, _data.BaseHP, _data.HPPerLevel);
        Resource = new ResourceManager(EntityID, _data.BaseResource, _data.ResourcePerLevel);
        AttackDamage = new ResourceManager(EntityID, _data.BaseAttackDamage, _data.AttackPerLevel);

        Experience = new ExperienceManager(EntityID, _data.BaseLevel, _data.MaxXPScaler);
        Experience.OnLevelUp += Levelup;
               
        agent = GetComponent<NavMeshAgent>();
        /*agent.agentTypeID = Entity_Sizes.Champion*/
        agent.speed = _data.BaseMoveSpeed;
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
    }
}
