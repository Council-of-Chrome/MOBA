using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ChampionData test; //modify this between champion and minion data for testing

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
            return Entities[Team_Type.Blue][_entityID];
        if (Entities[Team_Type.Red].ContainsKey(_entityID))
            return Entities[Team_Type.Blue][_entityID];
        throw new System.Exception("Entity not registered.");
    }

    private void Start()
    {
        int id = EntityFactory.Instance.SpawnChampion(test, Team_Type.Blue).EntityID; //use this for spawning and other move commands
        //(Entities[Team_Type.Blue][id] as IManageNavAgent).MoveTo(new Vector3(5f, 0f, 5f));
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
