using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class TargetFetching
{
    public static IEntityTargetable[] FetchAOE(Vector3 _pos, float _angle, float _radius, Vector3 _forward, TeamMask _mask)
    {
        List<IEntityTargetable> hits = new List<IEntityTargetable>();

        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if(team.Value)
                foreach (IEntityTargetable target in GameManager.Entities[team.Key].Values)
                {
                    if (target is IManageNavAgent)
                    {
                        Vector3 targetPos = (target as IManageNavAgent).GetPosition();
                        Vector3 distance = (_pos - targetPos).normalized;

                        if (Vector3.Dot(distance, _forward.normalized) > (1 - (_angle * (1 / 360) * 2)))
                        {
                            //within sector
                            hits.Add(target);
                        }
                    }
                }
        }
        return hits.ToArray();
    }
}
