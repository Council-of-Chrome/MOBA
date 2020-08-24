using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public MinionController SpawnMinion(MinionData _data, Team_Type _team)
    {
        GameObject go = Instantiate(MinionPrefab, Vector3.zero, Quaternion.identity);
        go.name = $"Minion |{_data.DisplayName}| ID |{entityCounter}|";

        MinionController mc = go.GetComponent<MinionController>();
        mc.Initialize(entityCounter, _data);

        GameManager.Entities[_team].Add(entityCounter, mc);

        entityCounter++;

        return mc;
    }

    public ChampionController SpawnChampion(ChampionData _data, Team_Type _team)
    {
        //TODO: rotate spawns around appropriate team fountain
        GameObject go = Instantiate(ChampionPrefab, Vector3.zero, Quaternion.identity);
        go.name = $"Champion |{_data.DisplayName}|";

        ChampionController cc = go.GetComponent<ChampionController>();
        cc.Initialize(entityCounter, _data);

        GameManager.Entities[_team].Add(entityCounter, cc);

        entityCounter++;

        return cc;
    }
}
