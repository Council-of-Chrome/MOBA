using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Entity_Type { Champion, RangedMinion, MeleeMinion, SiegeMinion }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static Dictionary<int, IEntityManager> entities = new Dictionary<int, IEntityManager>();

    public GameObject[] EntityPrefabs;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    public void SpawnEntity(int _id, int _entityTypeID, ref NetworkPacket _packet)
    {
        Vector3 pos = _packet.ReadVector3();
        Quaternion rot = _packet.ReadQuaternion();

        GameObject go = Instantiate(EntityPrefabs[_entityTypeID], pos, rot);

        switch ((Entity_Type)_entityTypeID)
        {
            case Entity_Type.Champion:
                IChampionManager champ = go.GetComponent<ChampionController>() as IChampionManager;

                champ.EntityID = _id;
                champ.Username = _packet.ReadString();
                champ.InitVisuals(_packet.ReadInt());

                entities.Add(_id, champ);
                break;
        }
    }

}