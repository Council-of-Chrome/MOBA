using System.Collections;
using UnityEngine;

public interface ITargetSelf
{

}
public interface ITargetAOE
{
    float Radius { get; }
    int Angle { get; }
}
public interface ITargetLinear
{
    float Length { get; }
    float Width { get; }
}
public interface ITargetVector
{
    Vector3 StartPos { get; }
    Vector3 EndPos { get; }

    float Width { get; }
}

public interface IAffectHealth
{
    ResourceEffector[] HealthEffectorPerLevel { get; }
}
public interface IAffectResource
{
    ResourceEffector ResourceEffector { get; }
}