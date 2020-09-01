using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public bool Casting { get; private set; } = false;

    public int AbilityPointsPool { get; private set; } = 0;

    private int EntityID { get; }
    private ChampionAbilityContainer[] abilities;

    public AbilityManager(int _entityID, Ability[] _abilities)
    {
        EntityID = _entityID;

        abilities = new ChampionAbilityContainer[_abilities.Length];
        for (int i = 0; i < abilities.Length; i++)
        {
            abilities[i] = new ChampionAbilityContainer(_abilities[i]);
        }
    }

    public void Trigger(int _index, Ray _mouseRay)
    {
        if (!Casting)
        {
            Casting = true;
            abilities[_index].Trigger(_mouseRay);
        }
    }

    public void Levelup(int _level)
    {
        AbilityPointsPool++;
    }

    public void RankupAbility(int _entityLevel, int _abilityIndex)
    {
        if (AbilityPointsPool-- <= 0)
            return;

        if (Mathf.CeilToInt(_entityLevel * 0.5f) <= abilities[_abilityIndex].AbilityRank)
            abilities[_abilityIndex].RankUp();
    }
}
