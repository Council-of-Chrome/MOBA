using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraManager
{
    public float TotalAura { get { return Base + CoreAura + DynamicAura; } }

    public float DynamicAura { get; private set; }
    public float CoreAura { get; private set; }

    public AuraSplitter Split { get; private set; }

    private int EntityID { get; }

    private float CoreAuraPerLevel { get; }
    private float DynamicAuraPerLevel { get; }

    private float Base { get { return BaseCore + BaseDynamic; } }
    private float BaseCore { get; }
    private float BaseDynamic { get; }

    public AuraManager(int _entityID, float _baseCore, float _baseDynamic, float _coreAuraPerLevel, float _dynAuraPerLevel)
    {
        EntityID = _entityID;

        BaseCore = _baseCore;
        BaseDynamic = _baseDynamic;

        CoreAuraPerLevel = _coreAuraPerLevel;
        DynamicAuraPerLevel = _dynAuraPerLevel;

        Split = new AuraSplitter(BaseDynamic * 0.5f, BaseDynamic * 0.5f);
    }

    public void Levelup(int _newLevel)
    {
        CoreAura = BaseCore + (CoreAuraPerLevel * (_newLevel - 1));
        DynamicAura = BaseDynamic + (DynamicAuraPerLevel * (_newLevel - 1));
    }

    public void Update(float _physical, float _magical)
    {
        float total = _physical + _magical;

        float physPercent = _physical / total;
        float magiPercent = 1f - physPercent;

        Split = new AuraSplitter(CoreAura + (physPercent * DynamicAura), CoreAura + (magiPercent * DynamicAura));
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
