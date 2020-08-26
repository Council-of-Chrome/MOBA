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

        Entities = new Dictionary<Team_Type, Dictionary<int, IEntityTargetable>>
        {
            [Team_Type.Blue] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Red] = new Dictionary<int, IEntityTargetable>(),
            [Team_Type.Neutral] = new Dictionary<int, IEntityTargetable>()
        };
    }
    #endregion   //may not actually need this

    public static Dictionary<Team_Type, Dictionary<int, IEntityTargetable>> Entities;

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

    public static IEntityTargetable GetEntity(int _entityID)
    {
        if (Entities[Team_Type.Blue].ContainsKey(_entityID))
            return Entities[Team_Type.Blue][_entityID];
        if (Entities[Team_Type.Neutral].ContainsKey(_entityID))
            return Entities[Team_Type.Neutral][_entityID];
        if (Entities[Team_Type.Red].ContainsKey(_entityID))
            return Entities[Team_Type.Red][_entityID];
        throw new System.Exception("Entity not registered.");
    }

    public static bool EntityIsTeam(int _entityID, Team_Type _team)
    {
        if (GetTeamOf(_entityID) == _team)
            return true;
        return false;
    }

    public static Team_Type GetTeamOf(int _entityID)
    {
        if (Entities[Team_Type.Blue].ContainsKey(_entityID))
            return Team_Type.Blue;
        if (Entities[Team_Type.Neutral].ContainsKey(_entityID))
            return Team_Type.Neutral;
        if (Entities[Team_Type.Red].ContainsKey(_entityID))
            return Team_Type.Red;
        throw new System.Exception("Entity not registered.");
    }

    private void Start() //uncomment to test stuff
    {
        int idc = EntityFactory.Instance.SpawnChampion(test1, Team_Type.Blue, Vector3.zero).EntityID; //use this for spawning and other move commands
        EntityFactory.Instance.SpawnMinion(test2, Team_Type.Red, Vector3.forward * 3);
        (Entities[Team_Type.Blue][idc] as IManageAbilities).CastAbility(new bool[4] { true, false, false, false }, Vector3.forward);
        (Entities[Team_Type.Blue][idc] as IManageNavAgent).MoveTo(new Vector3(5f, 0f, 5f));
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
