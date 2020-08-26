using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAbility1 : Ability, IAbilityCastable, ITargetAOE, IAffectHealth
{
    public override string AbilityName => "Sample Ability";
    public override string Description => "Does sample shit yo";

    public float[] CooldownPerLevel { get; }
    public float[] CostPerLevel { get; }

    public TeamMask Mask { get; } = new TeamMask(true, true, true);

    [SerializeField]
    private float radius = 5;
    public float Radius { get { return radius; } }
    public int Angle { get; } = 90;

    public ResourceEffector HealthEffector { get; }

    public void Trigger(int _casterID, Vector3 _targetPos)
    {
        //start animation or smth
        IManageNavAgent self = GameManager.GetEntity(_casterID) as IManageNavAgent;

        Vector3 forwardVec = (_targetPos - self.GetPosition()).normalized;
        IEntityTargetable[] hits = TargetFetching.FetchAOE(self.GetPosition(), Angle, Radius, forwardVec, Mask);

        foreach (IEntityTargetable target in hits)
        {
            if ((target as IManageHealth) == target)
                Debug.Log("diff interfaces still true");

            if (target is IManageHealth)
            {
                (target as IManageHealth).Health.Modify(HealthEffector);
            }
        }
    }
}
