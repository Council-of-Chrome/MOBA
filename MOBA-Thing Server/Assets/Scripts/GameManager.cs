using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ChampionData test1; //modify this between champion and minion data for testing
    public MinionData test2; //modify this between champion and minion data for testing

    #region Singleton
    public static GameManager Instance;
    private void OnEnable()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            throw new System.Exception("GameManager Instance already claimed.");
        }
        Instance = this;

        entities = new Dictionary<Team_Type, Dictionary<int, IEntityTargetable>>
        {
            [Team_Type.Blue] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Red] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Neutral] = new Dictionary<int, IEntityTargetable>()
        };
    }
    #endregion   //may not actually need this

    private static Dictionary<Team_Type, Dictionary<int, IEntityTargetable>> entities;

    public static float MatchTimeMilliseconds { get; private set; } = 0f;

    public static GameClock GameTime { get { return new GameClock(MatchTimeMilliseconds); } }
    public struct GameClock
    {
        public int Minutes { get; private set; }
        public int Seconds { get; private set; }

        public GameClock(float _time)
        {
            Minutes = Mathf.RoundToInt(_time * 0.016f); //0.016f ~= 1f / 60f, faster for compiler
            Seconds = Mathf.RoundToInt(_time % 60f);
        }
    }


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

    private void Start() //uncomment to test stuff
    {
        int idc = EntityFactory.Instance.SpawnChampion(test1, Team_Type.Blue, Vector3.zero).EntityID; //use this for spawning and other move commands
        EntityFactory.Instance.SpawnMinion(test2, Team_Type.Red, Vector3.forward * 3);
        (entities[Team_Type.Blue][idc] as IManageAbilities).CastAbility(new bool[4] { true, false, false, false }, new Ray(new Vector3(0f, 3f, 3f), Vector3.down));
        //(entities[Team_Type.Blue][idc] as IManageNavAgent).MoveTo(new Vector3(5f, 0f, 5f));
    }

    // Update is called once per frame
    void Update()
    {
        //do shit here probably

        MainThread.Update();
        MatchTimeMilliseconds += Time.unscaledDeltaTime;
        /*
        currentTime += Time.unscaledDeltaTime;
        if(currentTime >= 1f)
        {
            //send message to clients to update clock?
        }
        */
    }
}
