using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAbilityContainer
{
    public int AbilityRank { get; private set; }

    private Ability data;
    private int EntityID { get; }

    public ChampionAbilityContainer(Ability _data)
    {
        data = _data;
    }

    public void Trigger(Ray _mouseRay)
    {
        if (IsCastable())
            (data as IAbilityCastable).Trigger(EntityID, _mouseRay, AbilityRank);
    }

    public void RankUp()
    {
        AbilityRank = Mathf.Clamp(++AbilityRank, 0, 5);
    }

    #region Helpers
    public bool IsCastable()
    {
        return data is IAbilityCastable;
    }
    public bool IsPassive()
    {
        return data is IAbilityPassive;
    }
    #endregion  
}
