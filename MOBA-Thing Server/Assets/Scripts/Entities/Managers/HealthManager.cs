//plug these into monobehaviour controller classes to isolate work
using UnityEngine;

public class HealthManager
{
    public float Shield { get; private set; }
    public float Max { get; private set; }
    public float Current { get; private set; }
    public bool Invincible { get; set; }

    private int EntityID { get; }
    private float ResourcePerLvl { get; }
    private float Base { get; }

    public delegate void HealthModifiedHandler(HPModifiedInfo _info);
    public HealthModifiedHandler OnHealthModified;

    public HealthManager(int _entityID, float _baseHP, float _hpPerLvl, float _baseShield = 0f) 
    {
        EntityID = _entityID;

        Current = Max = Base = _baseHP;
        ResourcePerLvl = _hpPerLvl;
        Shield = _baseShield;
        Invincible = false;

        GameEventSystem.Sub<AffectHPInfo>(Modify);
    }
    ~HealthManager() //safety destructor
    {
        GameEventSystem.Unsub<AffectHPInfo>(Modify);
    }

    public void Levelup(int _level)
    {
        Max = Base + (ResourcePerLvl * (_level - 1));
        Current += ResourcePerLvl;
    }

    private void Modify(AffectHPInfo _info)
    {
        if(_info.TargetID != EntityID || Invincible)
            return;

        float toModify = 0f;
        if (_info.IgnoreShield || Shield == 0f)
            toModify = Current;
        else
            toModify = Current + Shield;

        float reducedVal = _info.Effector.Value;
        AuraSplitter splitter = (GameManager.GetEntity(EntityID) as IManageAuras).Auras.Split;

        switch (_info.Effector.Type)
        {
            case Damage_Type.Physical:
                reducedVal *= 1f / (1f + (splitter.PhysicalAura * 0.01f));
                break;
            case Damage_Type.Magical:
                reducedVal *= 1f / (1f + (splitter.MagicalAura * 0.01f));
                break;
        }

        switch (_info.Effector.StatType)
        {
            case Stat_Effector_Type.Flat:
                toModify += reducedVal;
                break;
            case Stat_Effector_Type.PMax:
                toModify += GetPercentMax(reducedVal);
                break;
            case Stat_Effector_Type.PMiss:
                toModify += GetPercentMissing(reducedVal);
                break;
            case Stat_Effector_Type.PCurrent:
                toModify += Current * reducedVal;
                break;
        }

        float diff = toModify - Current;
        if(Mathf.Sign(diff) == 1 && !_info.IgnoreShield)
        {
            float tmp = Shield;
            Shield = diff;
            OnHealthModified?.Invoke(new HPModifiedInfo(
                _info.CallerID,
                Current,
                Current,
                _info.Effector.Type,
                tmp,
                Shield
                ));
            Debug.Log($"ID damaged: {EntityID}, Damage dealer: {_info.CallerID}\nCurrent: {Current}, Shield: {Shield}");
            return;
        }

        float tmpShield = Shield;
        Shield = 0f;
        float tmpHP = Current;
        Current = toModify;
        OnHealthModified?.Invoke(new HPModifiedInfo(
            _info.CallerID,
            tmpHP,
            Current,
            _info.Effector.Type,
            tmpShield,
            Shield
            ));
        Debug.Log($"ID damaged: {EntityID}, Damage dealer: {_info.CallerID}\nCurrent: {Current}, Shield: {Shield}");
    }

    /// <summary>Gets percentage of max resource</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of max resource.</returns>
    public float GetPercentMax(float _percent)
    {
        return _percent * Max;
    }

    /// <summary>Gets percentage of missing resource</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of missing resource.</returns>
    public float GetPercentMissing(float _percent)
    {
        return GetPercentMax((Max - Current) * (1f / Max)) * _percent;
    }
}

public struct HPModifiedInfo //immutable poggers
{
    public int AttackerID { get; }

    public float OldHP { get; }
    public float NewHP { get; }

    public Damage_Type DamageType { get; }

    public float OldShield { get; }
    public float NewShield { get; }

    public HPModifiedInfo(int _attackerID, float _oldHP, float _newHP, Damage_Type _damageType, float _oldShield, float _newShield)
    {
        AttackerID = _attackerID;

        OldHP = _oldHP;
        NewHP = _newHP;

        DamageType = _damageType;

        OldShield = _oldShield;
        NewShield = _newShield;
    }
}


