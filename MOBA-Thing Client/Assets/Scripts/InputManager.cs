using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum User_Inputs
{
    MoveClick, AttackMoveClick, HoldPosition, StopAllInputs,
    ClassAbility1,
    ChampionAbility1, ChampionAbility2, ChampionAbility3, ChampionAbility4,
    RankUpAbility1, RankUpAbility2, RankUpAbility3, RankUpAbility4
}

public class InputManager : MonoBehaviour
{
    public static Dictionary<User_Inputs, Func<bool>> Inputs;

    // Start is called before the first frame update
    void OnEnable()
    {
        Inputs = new Dictionary<User_Inputs, Func<bool>>
        {
            { User_Inputs.MoveClick,        () => { return Input.GetMouseButtonDown(1); } },
            { User_Inputs.AttackMoveClick,  () => { return Input.GetKey(KeyCode.A); } },
            { User_Inputs.HoldPosition,     () => { return Input.GetKey(KeyCode.H); } },
            { User_Inputs.StopAllInputs,    () => { return Input.GetKey(KeyCode.S); } },

            { User_Inputs.ClassAbility1,    () => { return Input.GetKey(KeyCode.Alpha1); } },
            { User_Inputs.ChampionAbility1, () => { return Input.GetKey(KeyCode.Q); } },
            { User_Inputs.ChampionAbility2, () => { return Input.GetKey(KeyCode.W); } },
            { User_Inputs.ChampionAbility3, () => { return Input.GetKey(KeyCode.E); } },
            { User_Inputs.ChampionAbility4, () => { return Input.GetKey(KeyCode.R); } },

            { User_Inputs.RankUpAbility1,   () => { return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Q); } },
            { User_Inputs.RankUpAbility2,   () => { return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.W); } },
            { User_Inputs.RankUpAbility3,   () => { return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E); } },
            { User_Inputs.RankUpAbility4,   () => { return Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.R); } }
        };
    }

    // Update is called once per frame
    void Update()
    {

    }
}
