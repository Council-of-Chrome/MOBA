using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAbilityContainer
{
    private int EntityID { get; }
    public Ability Data;

    public ChampionAbilityContainer(Ability _data)
    {
        Data = _data;
    }

    public void Trigger(Vector3 _pos)
    {
        if (IsCastable())
            (Data as IAbilityCastable).Trigger(EntityID, _pos);
    }

    #region Helpers
    public bool IsCastable()
    {
        return Data is IAbilityCastable;
    }
    public bool IsPassive()
    {
        return Data is IAbilityPassive;
    }
    #endregion  
}
