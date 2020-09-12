using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class ClientHandle
{
    public static void Welcome(NetworkPacket _packet)
    {
        string msg = _packet.ReadString();
        int id = _packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.Instance.ID = id;
        ClientSend.WelcomeAcknowledgement();

        // Now that we have the client's id, connect UDP
        Client.Instance.Udp.Connect(((IPEndPoint)Client.Instance.Tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnEntity(NetworkPacket _packet)
    {
        GameManager.Instance.SpawnEntity(_packet.ReadInt(), _packet.ReadInt(), ref _packet);
    }

    public static void PlayerPosition(NetworkPacket _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.entities[_id].SetPosition(_packet.ReadVector3());
    }

    public static void PlayerRotation(NetworkPacket _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.entities[_id].SetRotation(_packet.ReadQuaternion());
    }
}