public enum Stat_Effector_Type { Flat, PMax, PMiss, PCurrent }

public class ResourceManager
{
    public float Shield { get; private set; }
    public float Max { get; private set; }
    public float Current { get; private set; }
    public bool Invincible { get; set; }

    public delegate float AffectResourceHandler(int _entityID, float _val); //<-- TODO: make more verbose than just 'id', contain team etc
    public AffectResourceHandler OnPreAffectResource;
    public AffectResourceHandler OnPostAffectResource;

    private int EntityID { get; }
    private float ResourcePerLvl { get; }

    public ResourceManager(int _entityID, float _baseResource, float _ResourcePerLvl)
    {
        EntityID = _entityID;

        Current = Max = _baseResource;
        ResourcePerLvl = _ResourcePerLvl;
        Invincible = false;
    }

    /// <summary>Handles all reduction and addition. Use negative values for reduction and positive for addition.</summary>
    /// <param name="_data"></param>
    /// <returns>Current resource after modification</returns>
    public virtual float Modify(ResourceEffector _data)
    {
        float value = _data.Value;

        if (Shield > _data.Value)
        {
            Shield -= value;
            return Current;
        }

        value -= Shield;
        Shield = 0f;

        if (OnPreAffectResource != null)
            value = OnPreAffectResource.Invoke(EntityID, _data.Value);

        if (Invincible)
            return Current;

        float temp = Current;

        switch (_data.Type)
        {
            case Stat_Effector_Type.Flat:
                Current += value;
                break;
            case Stat_Effector_Type.PMax:
                Current += GetPercentMax(value);
                break;
            case Stat_Effector_Type.PMiss:
                Current += GetPercentMissing(value);
                break;
            case Stat_Effector_Type.PCurrent:
                Current *= 1 - value;
                break;
        }

        OnPostAffectResource?.Invoke(EntityID, Current);
        return Current;
    }

    public virtual void Levelup(int _level)
    {
        Max = ResourcePerLvl * _level;
    }

    /// <summary>Gets percentage of max resource</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of max resource.</returns>
    public float GetPercentMax(float _percent)
    {
        return Max * (_percent * (1f / Max));
    }

    /// <summary>Gets percentage of missing resource</summary>
    /// <param name="_percent">The percent as a decimal.</param>
    /// <returns>Given percentage of missing resource.</returns>
    public float GetPercentMissing(float _percent)
    {
        return GetPercentMax(1f - (Current * (1f / Max)));
    }
}
