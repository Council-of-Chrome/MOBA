using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServerSend
{
    public static void Initialize()
    {
        HealthManager.OnHealthModified += SendHealthChange;
    }
    public static void Shutdown()
    {

    }

    private static void SendHealthChange(int _clientID, float _newHP, float _newShield)
    {

    }
}
