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

    public HealthManager(int _entityID, float _baseHP, float _hpPerLvl, float _baseShield = 0f) 
    {
        EntityID = _entityID;

        Current = Max = _baseHP;
        ResourcePerLvl = _hpPerLvl;
        Shield = _baseShield;
        Invincible = false;

        GameEventSystem.Sub<AffectHPInfo>(Modify);
    }

    public void Levelup(int _level)
    {
        Max = ResourcePerLvl * _level;
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

        switch (_info.Effector.Type)
        {
            case Stat_Effector_Type.Flat:
                toModify += _info.Effector.Value;
                break;
            case Stat_Effector_Type.PMax:
                toModify += GetPercentMax(_info.Effector.Value);
                break;
            case Stat_Effector_Type.PMiss:
                toModify += GetPercentMissing(_info.Effector.Value);
                break;
            case Stat_Effector_Type.PCurrent:
                toModify += Current * _info.Effector.Value;
                break;
        }

        float diff = toModify - Current;
        if(Mathf.Sign(diff) == 1 && !_info.IgnoreShield)
        {
            Shield = diff;
            Debug.Log($"ID damaged: {EntityID}, Damage dealer: {_info.CallerID}\nCurrent: {Current}, Shield: {Shield}");
            return;
        }

        Shield = 0f;
        Current = toModify;
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
