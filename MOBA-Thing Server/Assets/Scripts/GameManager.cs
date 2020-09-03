using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameClock))]
public class GameManager : MonoBehaviour
{
    public Texture2D VisionMap;

    public ChampionData test1; //modify this between champion and minion data for testing
    public MinionData test2; //modify this between champion and minion data for testing

    private TeamMask blueVisionMask = TeamMask.MaskToIgnoreAllies(Team_Type.Blue);
    private TeamMask redVisionMask = TeamMask.MaskToIgnoreAllies(Team_Type.Red);

    private void OnEnable()
    {
        Application.targetFrameRate = 60;
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
    //TODO: make plurals generic?
    public static IEntityTargetable[] GetEntities()
    {
        return GetEntities(TeamMask.MaskToHitAll());
    }
    public static IEntityTargetable[] GetEntities(TeamMask _mask)
    {
        List<IEntityTargetable> targets = new List<IEntityTargetable>();
        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if (team.Value)
                targets.AddRange(entities[team.Key].Values);
        }
        return targets.ToArray();
    }

    public static int GetEntityCount()
    {
        return entities[Team_Type.Blue].Count +
            entities[Team_Type.Neutral].Count +
            entities[Team_Type.Red].Count;
    }
    public static int GetEntityCount(TeamMask _mask)
    {
        int count = 0;
        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if (team.Value)
                count += entities[team.Key].Count;
        }
        return count;
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
        int idm = EntityFactory.Instance.SpawnMinion(test2, Team_Type.Red, Vector3.forward * 3).EntityID;

        (entities[Team_Type.Blue][idc] as IManageEXP).Levelup(1); //champion requires a level to use abilities

        AbilityManager test = (entities[Team_Type.Blue][idc] as IManageAbilities).Abilities;
        test.RankupAbility(1, 0); //same goes for ability rank
        test.Trigger(0, new Ray(new Vector3(0f, 3f, 3f), Vector3.down));

        (entities[Team_Type.Blue][idc] as IManageNavAgent).MoveTo(new Vector3(0f, 0f, 30f));
    }

    private void FixedUpdate()
    {
        DoVisionPass(blueVisionMask, ref VisionMap);
        DoVisionPass(redVisionMask, ref VisionMap);
    }

    List<IEntityTargetable> inVisionBlue = new List<IEntityTargetable>();
    List<IEntityTargetable> inVisionRed = new List<IEntityTargetable>();

    private static void DoVisionPass(TeamMask _mask, ref Texture2D _visionMap)
    {
        IEntityTargetable[] targets = GetEntities(_mask);

        IEntityTargetable[] allies = GetEntities(_mask.Flip());

        List<IEntityTargetable> targetsInVision = new List<IEntityTargetable>();

        foreach (IEntityTargetable ally in allies)
        {
            if(!(ally is IGrantVision))
                continue;

            foreach (IEntityTargetable target in targets)
            {
                Vector3 allyPos = ally.GetPosition(); //position isnt in texture space
                Vector2 allyPosFloored = new Vector2(allyPos.x + 40f, allyPos.z + 40f); // +40 moves 0,0 to bottom left corner || pos +40 /80 == normalized 0,0 bot left 1,1 top right 256 * normalized.floor() pos on vision map
                allyPosFloored /= 80;
                allyPosFloored *= 256; //<-- should be in correctly mapped unit space now

                Vector3 targetPos = target.GetPosition();
                Vector2 targetPosFloored = new Vector2(targetPos.x + 40f, targetPos.z + 40f);
                targetPosFloored /= 80;
                targetPosFloored *= 256;

                //Debug.Log($"ally x: {allyPosFloored.x}, ally y: {allyPosFloored.y} || target x: {targetPosFloored.x}, target y: {targetPosFloored.y}");
                if (IsInRangeOnVisionMap(allyPosFloored, targetPosFloored, ref _visionMap, (ally as IGrantVision).CurrentVisionRadius))
                    targetsInVision.Add(target);
            }
            Debug.Log($"{targetsInVision.Count}");
        }

        //TODO: get previous frame's array for desired team colour and check entities that are new to or no longer in the new list,
        //use this to then ping specified team's player's about modifying visibility client-side
    }

    static bool IsInRangeOnVisionMap(Vector2 _a, Vector2 _b, ref Texture2D _visionMap, int MaxVisionRadius)
    {
        if (GetDistanceSqrButVectors(_a, _b) > (MaxVisionRadius * MaxVisionRadius) * 2)
        {
            //Debug.Log(GetDistanceSqrButVectors(_a, _b));
            return false;
        }

        int w = (int)(_b.x - _a.x);
        int h = (int)(_b.y - _a.y);

        int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;

        if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
        if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
        if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;

        int longest = Mathf.Abs(w);
        int shortest = Mathf.Abs(h);

        if (!(longest > shortest))
        {
            int tmp = longest;
            longest = shortest;
            shortest = tmp;

            if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
            dx2 = 0;
        }

        int numerator = longest >> 1;

        int iteratorPosX = (int)_a.x;
        int iteratorPosY = (int)_a.y;

        for (int i = 0; i <= longest; i++)
        {
            //putpixel(x, y, color); <-- read texture 2d from here
            if (_visionMap.GetPixel(iteratorPosX, iteratorPosY).r == 0)
            {
                Debug.Log($"x: {iteratorPosX}, y: {iteratorPosY}, is black");
                return false;
            }

            numerator += shortest;
            if (!(numerator < longest))
            {
                numerator -= longest;
                iteratorPosX += dx1;
                iteratorPosY += dy1;
            }
            else
            {
                iteratorPosX += dx2;
                iteratorPosY += dy2;
            }
        }
        //this point comparison is legal, target is within vision of the other.
        return true;
    }
    static int GetDistanceSqrButVectors(Vector2 _a, Vector2 _b)
    {
        return (int)((_a.x - _b.x) * (_a.x - _b.x) + (_a.y - _b.y) * (_a.y - _b.y));
    }
}
