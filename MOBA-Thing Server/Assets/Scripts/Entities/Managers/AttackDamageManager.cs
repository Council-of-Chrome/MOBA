using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageManager
{
    public AttackDamageManager(int _entityID, float _baseAttack, float _attackPerLvl) { }

    public void Levelup(int _newLevel)
    {
        //Max = Base + dmgperlevel * _newLevel - 1;
    }
}
