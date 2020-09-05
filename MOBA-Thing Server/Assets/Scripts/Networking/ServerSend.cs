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

    public static void WritePacket<T>(Client_Packet_IDs _packetID, params object[] _params) where T : struct
    {
        using (NetworkPacket _packet = new NetworkPacket(_packetID))
        {

        }
    }
}

public static class SendData
{
    public static void TCPToClient(int _clientID, NetworkPacket _packet)
    {
        __packet.WriteLength();
        ServerBody.Clients[_clientID].SendDataByTCP(_packet);
    }
    public static void TCPToAll(NetworkPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < ServerBody.MaxPlayers; i++)
        {
            ServerBody.Clients[i].SendDataByTCP(_packet);
        }
    }
    public static void TCPToAll(int _exceptClient, NetworkPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < ServerBody.MaxPlayers; i++)
        {
            if (i == _exceptClient)
                continue;
            ServerBody.Clients[i].SendDataByTCP(_packet);
        }
    }

    public static void UDPToClient(int _clientID, NetworkPacket _packet)
    {
        _packet.WriteLength();
        ServerBody.Clients[_clientID].SendDataByUDP(_packet);
    }
    public static void UDPToAll(NetworkPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < ServerBody.MaxPlayers; i++)
        {
            ServerBody.Clients[i].SendDataByUDP(_packet);
        }
    }
    public static void UDPToAll(int _exceptClient, NetworkPacket _packet)
    {
        _packet.WriteLength();
        for (int i = 0; i < ServerBody.MaxPlayers; i++)
        {
            if (i == _exceptClient)
                continue;
            ServerBody.Clients[i].SendDataByUDP(_packet);
        }
    }
}