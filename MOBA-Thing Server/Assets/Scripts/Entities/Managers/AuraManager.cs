using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager
{
    public float TotalAura { get; private set; }
    public AuraSplitter Split { get; private set; }

    private int EntityID { get; }
    private float AuraPerLevel { get; }
    private float Base { get; }

    public AuraManager(int _entityID, float _baseAura, float _auraPerLevel)
    {
        EntityID = _entityID;
        TotalAura = Base = _baseAura;
        AuraPerLevel = _auraPerLevel;
        Split = new AuraSplitter(_baseAura / 2f, _baseAura / 2f);
    }

    public void Levelup(int _newLevel)
    {
        TotalAura = Base + (AuraPerLevel * (_newLevel - 1));
    }

    public void Update(float _physical, float _magical)
    {
        float total = _physical + _magical;

        float physPercent = _physical / total;
        float magiPercent = 1f - physPercent;

        Split = new AuraSplitter(physPercent * TotalAura, magiPercent * TotalAura);
        Debug.Log($"P:{Split.PhysicalAura}, M:{Split.MagicalAura}");
    }
}

public struct AuraSplitter
{
    public float PhysicalAura;
    public float MagicalAura;

    public AuraSplitter(float _physical, float _magical)
    {
        PhysicalAura = _physical;
        MagicalAura = _magical;
    }
}
