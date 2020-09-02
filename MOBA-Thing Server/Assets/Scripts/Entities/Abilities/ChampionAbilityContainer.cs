using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampionAbilityContainer
{
    public int AbilityRank { get; private set; } = 0;
    public Ability Data { get; }

    private int EntityID { get; }

    public ChampionAbilityContainer(Ability _data)
    {
        Data = _data;
    }

    public void Trigger(Ray _mouseRay)
    {
        if (IsCastable())
            (Data as IAbilityCastable).Trigger(EntityID, _mouseRay, AbilityRank);
    }

    public void RankUp(int _entityLevel)
    {
        if (Mathf.CeilToInt(_entityLevel * 0.5f) >= AbilityRank)
            AbilityRank = Mathf.Clamp(++AbilityRank, 0, 5);
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
