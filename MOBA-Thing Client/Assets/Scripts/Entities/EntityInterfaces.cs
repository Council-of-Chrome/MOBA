using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityManager
{
    int EntityID { get; set; }
    void SetPosition(Vector3 _newPos);
    void SetRotation(Quaternion _newRot);
    void InitVisuals(int _visualsID);
}

public interface IChampionManager : IEntityManager
{
    string Username { get; set; }
}
