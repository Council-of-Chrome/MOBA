using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ChampionData test1; //modify this between champion and minion data for testing
    public MinionData test2; //modify this between champion and minion data for testing

    private void OnEnable()
    {
        entities = new Dictionary<Team_Type, Dictionary<int, IEntityTargetable>>
        {
            [Team_Type.Blue] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Red] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Neutral] = new Dictionary<int, IEntityTargetable>()
        };
    }


    #region Entity Methods
    private static Dictionary<Team_Type, Dictionary<int, IEntityTargetable>> entities;

    public static void RegisterEntity(Team_Type _team, int _entityID, IEntityTargetable _entity)
    {
        entities[_team].Add(_entityID, _entity);
    }

    public static IEntityTargetable GetEntity(int _entityID)
    {
        if (entities[Team_Type.Blue].ContainsKey(_entityID))
            return entities[Team_Type.Blue][_entityID];
        if (entities[Team_Type.Neutral].ContainsKey(_entityID))
            return entities[Team_Type.Neutral][_entityID];
        if (entities[Team_Type.Red].ContainsKey(_entityID))
            return entities[Team_Type.Red][_entityID];
        throw new System.Exception("Entity not registered.");
    }

    public static IEntityTargetable[] GetEntities()
    {
        IEntityTargetable[] targets = new IEntityTargetable[GetEntityCount()];
        entities[Team_Type.Blue].Values.CopyTo(targets, 0);
        entities[Team_Type.Neutral].Values.CopyTo(targets, GetEntityCount(Team_Type.Blue));
        entities[Team_Type.Red].Values.CopyTo(targets, GetEntityCount(Team_Type.Blue) + GetEntityCount(Team_Type.Neutral));
        return targets;
    }
    public static IEntityTargetable[] GetEntities(Team_Type _team)
    {
        IEntityTargetable[] targets = new IEntityTargetable[GetEntityCount(_team)];
        entities[_team].Values.CopyTo(targets, 0);
        return targets;
    }

    public static int GetEntityCount()
    {
        return entities[Team_Type.Blue].Count +
            entities[Team_Type.Neutral].Count +
            entities[Team_Type.Red].Count;
    }
    public static int GetEntityCount(Team_Type _team)
    {
        return entities[_team].Count;
    }

    public static bool EntityIsTeam(int _entityID, Team_Type _team)
    {
        if (GetTeamOf(_entityID) == _team)
            return true;
        return false;
    }

    public static Team_Type GetTeamOf(int _entityID)
    {
        if (entities[Team_Type.Blue].ContainsKey(_entityID))
            return Team_Type.Blue;
        if (entities[Team_Type.Neutral].ContainsKey(_entityID))
            return Team_Type.Neutral;
        if (entities[Team_Type.Red].ContainsKey(_entityID))
            return Team_Type.Red;
        throw new System.Exception("Entity not registered.");
    }
    #endregion  

    private void Start() //uncomment to test stuff
    {
        int idc = EntityFactory.Instance.SpawnChampion(test1, Team_Type.Blue, Vector3.zero).EntityID; //use this for spawning and other move commands
        EntityFactory.Instance.SpawnMinion(test2, Team_Type.Red, Vector3.forward * 3);
        (entities[Team_Type.Blue][idc] as IManageAbilities).CastAbility(new bool[4] { true, false, false, false }, new Ray(new Vector3(0f, 3f, 3f), Vector3.down));
        //(entities[Team_Type.Blue][idc] as IManageNavAgent).MoveTo(new Vector3(5f, 0f, 5f));
    }
}
