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

    public void Trigger(Ray _mouseRay)
    {
        if (IsCastable())
            (Data as IAbilityCastable).Trigger(EntityID, _mouseRay);
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
