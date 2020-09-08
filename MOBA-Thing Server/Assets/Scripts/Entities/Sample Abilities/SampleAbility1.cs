using UnityEngine;

public class SampleAbility1 : Ability, IAbilityCastable, ITargetAOE, IAffectHealth
{
    public override string AbilityName => "Sample Ability";
    public override string Description => "Does sample shit yo";

    public float CastTime { get; } = 0f;
    public float[] CooldownPerLevel { get; }
    public Timer CooldownTimer { get; private set; }
    public float[] CostPerLevel { get; }

    public TeamMask Mask { get; private set; }

    [SerializeField]
    private float radius = 5;
    public float Radius { get { return radius; } }
    public int Angle { get; } = 360;
    [SerializeField]
    private ResourceEffector[] healthEffectorPerLevel = default;
    public ResourceEffector[] HealthEffectorPerLevel { get { return healthEffectorPerLevel; } }

    public void Trigger(int _casterID, Ray _mouseRay, int _abilityRank)
    {
        //start animation or smth
        IEntityTargetable self = GameManager.GetEntity(_casterID);

        Mask = TeamMask.MaskToIgnoreAllies(GameManager.GetTeamOf(_casterID));

        //used for getting the location of a multihit targeting system based on mouse ray
        Vector3 targetPoint = TargetFetching.GetPointOn0PlaneFromRay(_mouseRay);
        Vector3 forwardVec = (targetPoint - self.GetPosition()).normalized;
        IEntityTargetable[] hit = TargetFetching.FetchAOE(targetPoint, 360f, 5f, forwardVec, Mask);

        foreach (IEntityTargetable target in hit) //multi hit effect sample
        {
            if (target == self)
                continue;

            GameEventSystem.TriggerGameEvent(new AffectHPInfo(
                target.EntityID,
                _casterID,
                false,
                HealthEffectorPerLevel[_abilityRank - 1]
                ));
        }


    }
}
