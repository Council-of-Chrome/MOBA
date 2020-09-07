﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GameClock))]
public class GameManager : MonoBehaviour
{
    public static List<int> InVisionBlue = new List<int>();
    public static List<int> InVisionRed = new List<int>();

    public Texture2D VisionMap;

    public ChampionData test1; //modify this between champion and minion data for testing
    public MinionData test2; //modify this between champion and minion data for testing

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
        Vector3 spawnpos1 = TranslateToWorldCoords(new Vector2(119f, 203f));
        Vector3 spawnpos2 = TranslateToWorldCoords(new Vector2(123f, 156f));

        int idc1 = EntityFactory.Instance.SpawnChampion(test1, Team_Type.Blue, spawnpos1).EntityID; //use this for spawning and other move commands
        int idc2 = EntityFactory.Instance.SpawnChampion(test1, Team_Type.Red, spawnpos2).EntityID; //use this for spawning and other move commands

        //int idm = EntityFactory.Instance.SpawnMinion(test2, Team_Type.Red, Vector3.forward * 3).EntityID;

        //(entities[Team_Type.Blue][idc] as IManageEXP).Levelup(1); //champion requires a level to use abilities

        //AbilityManager test = (entities[Team_Type.Blue][idc] as IManageAbilities).Abilities;
        //test.RankupAbility(1, 0); //same goes for ability rank
        //test.Trigger(0, new Ray(new Vector3(0f, 3f, 3f), Vector3.down));

        //(entities[Team_Type.Blue][idc] as IManageNavAgent).MoveTo(new Vector3(-40f, 0f, -40f));

    }

    private void FixedUpdate()
    {
        //TODO: only update the vision pass if some entity has declared it is going to move, should
        //reduce weight
        DoVisionPass(Team_Type.Blue, ref VisionMap);
        DoVisionPass(Team_Type.Red, ref VisionMap);
    }

    private static void DoVisionPass(Team_Type _team, ref Texture2D _visionMap)
    {
        //                              ,,..
        //                            ,@$$$$$.
        //                          .,$$$$$$$$i
        //                    .,z$""')$$$$$$$$C`^#`-..
        //                 ,zF'        `""#*"'       "*o.
        //              ,zXe > u:..        ..      "c
        //            ,' zP'    ,:`"          .            "N.
        //          ,d",d$   ,'"   ,uB" .,uee..,?R.  ,  .    ^$.
        //        ,@P d$"     .:$$$$$$$$$$$$$@$CJN.,"    `     #b
        //       z$" d$P    :SM$$$$$$$$$$$$$$$$$$$Nf.           ^$.
        //      J$" J$P  , ,@$$$$$$$$$$$$$$$$$$$$$$$$$k.         "$r
        //     z$   $$.   ,$$$$$$$$$$$$$$$$$$$$$$$$$$f'   .    .   $b
        //    ,$"  $$u,-.x'^""$$$$$$$$$$$$$$$$$$$$$$$$$.        `.  $k
        //    $"  :$$$$> 8.   `#$$$$$$$$$$$$$$$$$$$$$"\  d.F   $.
        //   $P.$$$$$N `$b.  $$$$$$$$$$$$$$$$$$$$$k.$  $"  :   '   `$
        //  {$'  4$$k $$c `*$.,Q$$$$$$$$$$$$$$$$$$$$$$$ ..            $L
        //  $P   4$$$$$F:   `"$$$$$$$$$$$$$$$$$$$$$$'`$".   ,    `$
        // ,$'  ,$$$$$d$$    '##$$c3$$$$$$$$$$$$$$$$. '      :   L.    $.
        // J$  u$$$$$$$$$.,oed$*$$$$N "#$$$$$$$$$$***$@$N. , $  ,B$$N.,9L
        // $F,$$$$$$$$$$,@*"'  `J$$$$$#h$$$$$$P"`     `"*$$. $4W$' "$$uJF
        // 4$$$$$$$$$$$$F'      $*'`$$RR@$$$$$R        ,' "$d$4"    '$$$R
        //,$$$$$$$$$$$$$F     ,'    @$.3$$$$ R>            `$F$  dN.4$$$$.
        //$$$$$$$$$$$$*$"          J$'$$$$$& $.             $'   $$$$$$$$$o
        //^$$$$$$$$$$B@$$          $P $$$"?N/$k             $r   $$P" *$$$$'
        //  $$i.$$$$"$'         $$ ~R$P '$k^$$,'          $   "'  ,d$$'
        //  $$$$ J$$$$ `,'    .,z$P'd.$P   #$. #$$u.       .$  eu. ,d$$$
        //  $^$$$$$$$$. `"=+=N#'.,d$M$$'   `$$@s.#$$$u.   ,$C  $$$@$$$"$
        //  "  `*$$$$$$bx..        ,M$"     `*$$$b/""$R"*"'d$ ,$$$$P"  '
        //  4     "$$k3$9$$B.e.  ,ud$F       `3$$$$b.      ,$,@R$*'    4
        //  {       *$$$$$$$b$$@$$$$$L   ,.  ,J$$.'**$$k$NX$"M"'       .
        //  $         "$#"  `" {$$$$$$c,z$N.,o$$$$   ,NW$*"'           $
        //  $.         ',    `$$$$$$$$$d$$$$$$$$$f ,$e*'               $
        // ,$c         d.     `^$$$$$$$$$$$$$$$$$.u '"                :$.
        // $$$         $\   .,  `"#$$$*$$$$$$$$$$$$ '                 4$F
        // $$"         $ `  k.`.     ``"#`"""'      ,' ,'             `$$
        // `"          $>,  `b.,ce(b:o uz CCLd$4$*F?\,o                "'
        //             $&    $$k'*"$$$$$$#$$$$$$$$$$ d'
        //             $$.,$$$$$$$$,e,$#$.*$`""""'e4 $
        //             `$$$$  ^$$\$"$$$$$$$$$$$$$$$.eL
        //              $$$"  $$$$$$$e$.$.$$.$e$d$$$$k
        //              R`$$  '$$$$$$$$$$$$$$$$$$$$$$P
        //              `  $Nc'"$$N3$$$$$$$$$$$$$$$$$'
        //                  *$  9$.`@$$$$$$$$$$R$$$#'
        //                   `$.  `"*$$$$$$$$$$P'' #
        //                     "$u.    `""""''   ,'
        //                       `"$Nu..  .,z{p"'
        //                           `"####""'
        // BE WARNED: this shit gets fucking whacky, fucking with this may
        // potentially fuck up vision registration of all clients and 
        // also hit detection. Fuck with at your own risk.

        TeamMask mask = TeamMask.MaskToIgnoreAllies(_team);

        IEntityTargetable[] targets = GetEntities(mask);
        IEntityTargetable[] allies = GetEntities(mask.Flip());

        List<int> targetsInVision = new List<int>();

        foreach (IEntityTargetable ally in allies)
        {
            if(!(ally is IGrantVision))
                continue;

            foreach (IEntityTargetable target in targets)
            {
                Vector2 allyPosVMap = TranslateToVisionMapCoords(ally.GetPosition());
                Vector2 targetPosVMap = TranslateToVisionMapCoords(target.GetPosition());

                //Debug.Log($"ally x: {allyPosFloored.x}, ally y: {allyPosFloored.y} || target x: {targetPosFloored.x}, target y: {targetPosFloored.y}");
                //Debug.DrawLine(allyPosFloored, targetPosFloored, Color.blue, .1f); <-- kinda just looks cool
                if (IsInRangeOnVisionMap(allyPosVMap, targetPosVMap, ref _visionMap, (ally as IGrantVision).CurrentVisionRadius))
                    targetsInVision.Add(target.EntityID);
            }
        }

        //get previous frame's array for desired team colour and check entities that are new to or no longer in the new list,
        //use this to then ping specified team's player's about modifying visibility client-side

        IEnumerable<int> leftVision;
        IEnumerable<int> enteredVision;

        switch (_team)
        {
            case Team_Type.Blue:
            {
                if (CompareLists(InVisionBlue, targetsInVision))
                    return;

                leftVision = InVisionBlue.Where(x => !targetsInVision.Contains(x));
                enteredVision = targetsInVision.Where(x => !InVisionBlue.Contains(x));

                //tell blue clients who entered and left
                foreach (int id in leftVision)
                {
                    (GetEntity(id) as IManageNavAgent).Agent.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.red;
                }
                foreach (int id in enteredVision)
                {
                    (GetEntity(id) as IManageNavAgent).Agent.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.yellow;
                }

                InVisionBlue = targetsInVision;
                break;
            }
            case Team_Type.Red:
            {
                if (CompareLists(InVisionRed, targetsInVision))
                    return;

                leftVision = InVisionRed.Where(x => !targetsInVision.Contains(x));
                enteredVision = targetsInVision.Where(x => !InVisionRed.Contains(x));

                //tell red clients who entered and left
                foreach (int id in leftVision)
                {
                    (GetEntity(id) as IManageNavAgent).Agent.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.blue;
                }
                foreach (int id in enteredVision)
                {
                    (GetEntity(id) as IManageNavAgent).Agent.transform.GetChild(1).GetComponent<MeshRenderer>().material.color = Color.cyan;
                }

                InVisionRed = targetsInVision;
                break;
            }
        }
    }

    static bool IsInRangeOnVisionMap(Vector2 _a, Vector2 _b, ref Texture2D _visionMap, int MaxVisionRadius)
    {
        if (GetDistanceSqr(_a, _b) > (MaxVisionRadius * MaxVisionRadius) * 2)
            return false;

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

        float startPixelCol = _visionMap.GetPixel(iteratorPosX, iteratorPosY).r; //r is 0-1 scale
        bool prevPixelIsBush = startPixelCol != 0f && startPixelCol != 1f;

        for (int i = 0; i <= longest; i++)
        {
            float pixelCol = _visionMap.GetPixel(iteratorPosX, iteratorPosY).r; //r is 0-1 scale

            if (pixelCol == 0f) //is wall
                return false;

            if (pixelCol != 0f && pixelCol != 1f) //is bush
            {
                if (!prevPixelIsBush) //treat bush like a wall 
                    return false;

                prevPixelIsBush = true;
            }
            else
            {
                prevPixelIsBush = false;
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
    static int GetDistanceSqr(Vector2 _a, Vector2 _b)
    {
        return (int)((_a.x - _b.x) * (_a.x - _b.x) + (_a.y - _b.y) * (_a.y - _b.y));
    }

    static bool CompareLists<T>(List<T> _a, List<T> _b)
    {
        if (_a == null || _b == null || _a.Count != _b.Count)
            return false;

        if (_a.Count == 0)
            return true;

        Dictionary<T, int> lookUp = new Dictionary<T, int>();

        // create index for the first list
        for (int i = 0; i < _a.Count; i++)
        {
            if (!lookUp.TryGetValue(_a[i], out int count))
            {
                lookUp.Add(_a[i], 1);
                continue;
            }
            lookUp[_a[i]] = count + 1;
        }

        for (int i = 0; i < _b.Count; i++)
        {
            if (!lookUp.TryGetValue(_b[i], out int count))
            {
                // early exit as the current value in B doesn't exist in the lookUp (and not in ListA)
                return false;
            }
            count--;
            if (count <= 0)
                lookUp.Remove(_b[i]);
            else
                lookUp[_b[i]] = count;
        }
        // if there are remaining elements in the lookUp, that means ListA contains elements that do not exist in ListB
        return lookUp.Count == 0;
    }

    static Vector2 TranslateToVisionMapCoords(Vector3 _worldPos)
    {
        return (new Vector3(_worldPos.x + 40f, _worldPos.z + 40f) / 80) * 256;
    }
    static  Vector3 TranslateToWorldCoords(Vector2 _vMapPos)
    {
        Vector3 final = (new Vector3(_vMapPos.x, 0f, _vMapPos.y) / 256) * 80;
        final.x -= 40f;
        final.z -= 40f;

        return final;
    }
}
