using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public bool Casting { get; private set; } = false;

    public int AbilityPointsPool { get; private set; } = 0;

    private int EntityID { get; }
    private ChampionAbilityContainer[] abilities;
    private Timer castTimer;

    public AbilityManager(int _entityID, Ability[] _abilities)
    {
        EntityID = _entityID;

        abilities = new ChampionAbilityContainer[_abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new ChampionAbilityContainer(_abilities[i]);
        }
        castTimer = new Timer(null, null, () => { Casting = false; });
    }

    public void Trigger(int _index, Ray _mouseRay)
    {
        if (abilities[_index].IsCastable() && abilities[_index].AbilityRank > 0)
        {
            if (!Casting)
            {
                float castTime = (abilities[_index].Data as IAbilityCastable).CastTime;
                castTimer.Reset(castTime); //reset calls Stop then Start(), meaning it will set Casting to false. :- set Casting to true AFTER calling reset.
                Casting = true;
                abilities[_index].Trigger(_mouseRay);
            }
        }
    }

    public void Levelup(int _level)
    {
        AbilityPointsPool = _level;
    }

    public void RankupAbility(int _entityLevel, int _abilityIndex)
    {
        if (AbilityPointsPool-- <= 0)
            return;

        abilities[_abilityIndex].RankUp(_entityLevel);
    }
}
