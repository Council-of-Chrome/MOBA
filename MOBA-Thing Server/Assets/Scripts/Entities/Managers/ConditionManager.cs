using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICondition
{
    void Init(int _entityID);
}

public class ConditionManager
{
    private List<ICondition> conditions = new List<ICondition>();

    private int EntityID { get; }

    public ConditionManager(int _entityID)
    {
        EntityID = _entityID;
    }

    public void AddCondition(ICondition _toAdd) //conditions handle removing themselves
    {
        conditions.Add(_toAdd);
        _toAdd.Init(EntityID);
    }
}
