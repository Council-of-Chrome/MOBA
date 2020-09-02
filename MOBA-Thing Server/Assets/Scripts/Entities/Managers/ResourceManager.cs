using UnityEngine;

public enum Stat_Effector_Type { Flat, PMax, PMiss, PCurrent }

public class ResourceManager
{
    public float Max { get; private set; }
    public float Current { get; private set; }

   //public delegate float AffectResourceHandler(int _entityID, float _val); //<-- TODO: make more verbose than just 'id', contain team etc
   //public static AffectResourceHandler OnPreAffectResource;
   //public static AffectResourceHandler OnPostAffectResource;

    private int EntityID { get; }
    private float ResourcePerLvl { get; }

    public ResourceManager(int _entityID, float _baseResource, float _ResourcePerLvl)
    {
        EntityID = _entityID;

        Current = Max = _baseResource;
        ResourcePerLvl = _ResourcePerLvl;
    }

    /// <summary>Handles all reduction and addition. Use negative values for reduction and positive for addition.</summary>
    /// <param name="_data"></param>
    /// <returns>Current resource after modification</returns>
    public virtual float Modify(ResourceEffector _data)
    {
        float value = _data.Value;

        //if (GameEventSystem.OnPreAffectResource != null)
        //    value = GameEventSystem.OnPreAffectResource.Invoke(EntityID, _data.Value);

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
                Current += Current * value;
                break;
        }

        //GameEventSystem.OnPostAffectResource?.Invoke(EntityID, Current);
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
