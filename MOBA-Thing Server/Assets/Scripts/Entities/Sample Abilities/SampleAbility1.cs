using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAbility1 : Ability, IAbilityCastable, ITargetAOE, IAffectHealth
{
    public override string AbilityName => "Sample Ability";
    public override string Description => "Does sample shit yo";

    public float[] CooldownPerLevel { get; }
    public float[] CostPerLevel { get; }

    public TeamMask Mask { get; }

    public float Radius { get; }
    public int Angle { get; }

    public ResourceEffector HealthEffector { get; }


    public IEnumerator Trigger(int _casterID, Vector3 _targetPos)
    {
        //start animation or smth
        yield return new WaitForSecondsRealtime(0.75f);

        IManageNavAgent self = GameManager.GetEntity(_casterID) as IManageNavAgent;

        Vector3 forwardVec = (_targetPos - self.GetPosition()).normalized;
        IEntityTargetable[] hits = TargetFetching.FetchAOE(self.GetPosition(), Angle, Radius, forwardVec, Mask);

        foreach (IEntityTargetable target in hits)
        {
            if (target is IManageHealth)
                (target as IManageHealth).Health.Modify(HealthEffector);
        }
        yield return null;
    }
}
