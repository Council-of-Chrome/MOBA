﻿using UnityEngine;

public class EntityFactory : MonoBehaviour
{
    public static EntityFactory Instance;

    private void OnEnable()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            throw new System.Exception("EntityFactory instance already claimed.");
        }
        Instance = this;
    }

    [SerializeField]
    private GameObject ChampionPrefab;
    [SerializeField]
    private GameObject MinionPrefab;

    private static int entityCounter = 0;

    public MinionController SpawnMinion(MinionData _data, Team_Type _team, Vector3 _spawnPos)
    {
        GameObject go = Instantiate(MinionPrefab, _spawnPos, Quaternion.identity);
        go.name = $"Minion |{_data.DisplayName}| ID |{entityCounter}|";

        MinionController mc = go.GetComponent<MinionController>();
        mc.Initialize(entityCounter, _data);

        GameManager.RegisterEntity(_team, entityCounter, mc);

        entityCounter++;

        return mc;
    }

    public ChampionController SpawnChampion(ChampionData _data, Team_Type _team, Vector3 _spawnPos)
    {
        //TODO: rotate spawns around appropriate team fountain
        GameObject go = Instantiate(ChampionPrefab, _spawnPos, Quaternion.identity);
        go.name = $"Champion |{_data.DisplayName}|";

        ChampionController cc = go.GetComponent<ChampionController>();
        cc.Initialize(entityCounter, _data);

        GameManager.RegisterEntity(_team, entityCounter, cc);

        entityCounter++;

        return cc;
    }
}
