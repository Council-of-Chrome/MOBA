using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAbility1 : Ability, IAbilityCastable, ITargetAOE, IAffectHealth
{
    public override string AbilityName => "Sample Ability";
    public override string Description => "Does sample shit yo";

    public float[] CooldownPerLevel { get; }
    public float[] CostPerLevel { get; }

    public TeamMask Mask { get; private set; }

    [SerializeField]
    private float radius = 5;
    public float Radius { get { return radius; } }
    public int Angle { get; } = 360;

    public ResourceEffector HealthEffector { get; } = new ResourceEffector(-50, Stat_Effector_Type.Flat);

    public void Trigger(int _casterID, Vector3 _targetPos)
    {
        //start animation or smth
        IManageNavAgent self = GameManager.GetEntity(_casterID) as IManageNavAgent;

        Mask = TeamMask.MaskToIgnoreAllies(GameManager.GetTeamOf(_casterID));

        Vector3 forwardVec = (_targetPos - self.GetPosition()).normalized;
        IEntityTargetable[] hits = TargetFetching.FetchAOE(self.GetPosition(), Angle, Radius, forwardVec, Mask);

        foreach (IEntityTargetable target in hits)
        {
            if (target == self)
                continue;

            if (target is IManageHealth)
            {
                float b = (target as IManageHealth).Health.Current;
                float f = (target as IManageHealth).Health.Modify(HealthEffector);
                Debug.Log($"ID: {target.EntityID} HP: {b}, {f}");
            }
        }
    }
}
