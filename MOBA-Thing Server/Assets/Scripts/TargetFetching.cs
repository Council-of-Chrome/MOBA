using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class TargetFetching
{
    private const float SECTOR_MULT = 1f * (1f / 360f) * 2f;

    public static IEntityTargetable[] FetchAOE(Vector3 _pos, float _angle, float _radius, Vector3 _forward, TeamMask _mask)
    {
        List<IEntityTargetable> hits = new List<IEntityTargetable>();

        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if (team.Value)
                foreach (IEntityTargetable target in GameManager.GetEntities(team.Key))
                {
                    if (target is IManageNavAgent)
                    {
                        Vector3 targetPos = (target as IManageNavAgent).GetPosition();
                        Vector3 distance = (_pos - targetPos).normalized;

                        if(distance.sqrMagnitude <= Mathf.Pow(_radius, 2f))
                            if (Vector3.Dot(_forward.normalized, distance) <= _angle * SECTOR_MULT) //within sector
                                hits.Add(target);
                    }
                }
        }
        return hits.ToArray();
    }

    public static IEntityTargetable[] FetchBox(Vector3 _pos, float _xHalfExtents, float _zHalfExtents, TeamMask _mask)
    {
        List<IEntityTargetable> hits = new List<IEntityTargetable>();

        Vector2[] box = new Vector2[4]
        {    
            new Vector2(_pos.x - _xHalfExtents, _pos.z + _zHalfExtents),
            new Vector2(_pos.x + _xHalfExtents, _pos.z + _zHalfExtents),
            new Vector2(_pos.x + _xHalfExtents, _pos.z - _zHalfExtents),
            new Vector2(_pos.x - _xHalfExtents, _pos.z + _zHalfExtents)
        };

        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if (team.Value)
                foreach (IEntityTargetable target in GameManager.GetEntities(team.Key))
                {
                    if (target is IManageNavAgent)
                    {
                        Vector3 targetPos = (target as IManageNavAgent).GetPosition();

                        if (PointInPoly(box, targetPos))
                            hits.Add(target);
                    }
                }
        }
        return hits.ToArray();
    }

    public static IEntityTargetable[] FetchPolygon(Vector2[] _poly, TeamMask _mask)
    {
        if (_poly.Length < 3)
            throw new System.Exception("Poly requires minimum of 3 vertices.");

        List<IEntityTargetable> hits = new List<IEntityTargetable>();

        foreach (KeyValuePair<Team_Type, bool> team in _mask.Get())
        {
            if (team.Value)
                foreach (IEntityTargetable target in GameManager.GetEntities(team.Key))
                {
                    if (target is IManageNavAgent)
                    {
                        Vector3 targetPos = (target as IManageNavAgent).GetPosition();

                        if (PointInPoly(_poly, targetPos))
                            hits.Add(target);
                    }
                }
        }
        return hits.ToArray();
    }

    public static IEntityTargetable FetchSingle(Ray _mouseRay, TeamMask _mask)
    {
        if (Physics.Raycast(_mouseRay, out RaycastHit _hit, Mathf.Infinity))
        {
            foreach (MonoBehaviour mono in _hit.transform.GetComponentsInParent<MonoBehaviour>())
            {
                if (mono is IEntityTargetable target && _mask.Allows(GameManager.GetTeamOf(target.EntityID)))
                    return GameManager.GetEntity((mono as IEntityTargetable).EntityID);
            }
        }
        return null;
    }

    public static Vector3 GetPointOn0PlaneFromRay(Ray _mouseRay)
    {
        Plane plane0 = new Plane(Vector3.up, Vector3.zero);
        if (plane0.Raycast(_mouseRay, out float distance))
            return _mouseRay.GetPoint(distance);
        throw new System.Exception("Mouse ray failed to hit 0plane");
    }

    private static bool PointInPoly(Vector2[] _poly, Vector3 _targetPos)
    {
        Vector2 target = new Vector2(_targetPos.x, _targetPos.z);

        int lastVertIndex = _poly.Length - 1;
        float angle = GetAngle(_poly[0], target, _poly[lastVertIndex]);

        for (int i = 0; i < lastVertIndex; i++)
        {
            angle += GetAngle(_poly[i + 1], target, _poly[i]);
        }

        return (Mathf.Abs(angle) > 1);
    }
    private static float GetAngle(Vector2 _a, Vector2 _b, Vector2 _c)
    {
        Vector2 ba = new Vector2(_a.x - _b.x, _a.y - _b.y);
        Vector2 bc = new Vector2(_c.x - _b.x, _c.y - _b.y);

        float dot = Vector2.Dot(ba, bc);
        float crossLength = (ba.x * bc.y) - (ba.y * bc.x);

        return Mathf.Atan2(crossLength, dot);
    }
}
