public enum Damage_Type { Flat, PMax, PMiss, PCurrent }

public class Health
{
    public float Max { get; private set; }
    public float Current { get; private set; }
    public bool Invincible { get; set; }

    public delegate float AffectHealthHandler(int _entityID, float _val); //<-- TODO: make more verbose than just 'id', contain team etc
    public static AffectHealthHandler OnPreAffectHealth;
    public static AffectHealthHandler OnPostAffectHealth;

    private int EntityID { get; }
    private float HpPerLvl { get; }

    public Health(int _entityID, float _baseHP, float _hpPerLvl)
    {
        EntityID = _entityID;

        Current = Max = _baseHP;
        HpPerLvl = _hpPerLvl;
        Invincible = false;
    }

    /// <summary>Handles all damage and healing. Use negative values for damage and positive for healing.</summary>
    /// <param name="_data"></param>
    /// <returns>Current health after modification</returns>
    public float Modify(HealthEffector _data)
    {
        if (Invincible)
            return Current;

        float value = _data.Value;
        if(OnPreAffectHealth != null)
            value = OnPreAffectHealth.Invoke(EntityID, _data.Value);
        float temp = Current;

        switch (_data.Type)
        {
            case Damage_Type.Flat:
                Current += value;
                break;
            case Damage_Type.PMax:
                Current += GetPercentMax(value); 
                break;
            case Damage_Type.PMiss:
                Current += GetPercentMissing(value);
                break;
            case Damage_Type.PCurrent:
                Current *=  1 - value;
                break;
        }

        OnPostAffectHealth?.Invoke(EntityID, temp - Current);
        return Current;
    }

    public void Levelup(int _level)
    {
        Max += HpPerLvl * _level;
    }

    /// <summary>Gets percentage of max HP</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of max HP.</returns>
    public float GetPercentMax(float _percent)
    {
        return Max * (_percent * (1f / Max));
    }

    /// <summary>Gets percentage of missing HP</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of missing HP.</returns>
    public float GetPercentMissing(float _percent)
    {
        return GetPercentMax(1f - (Current * (1f / Max)));
    }
}
