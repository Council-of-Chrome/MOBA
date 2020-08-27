using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager
{
    public bool Casting { get; private set; } = false;

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

    public void Trigger(int _index, Vector3 _pos)
    {
        Casting = true;
        abilities[_index].Trigger(_pos);
    }
}
