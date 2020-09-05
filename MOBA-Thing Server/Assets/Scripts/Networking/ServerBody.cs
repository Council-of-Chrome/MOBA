using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class ServerBody
{
    public static int MaxPlayers { get; private set; }
    public static int Port { get; private set; }

    public static Dictionary<int, NetworkClient> Clients = new Dictionary<int, NetworkClient>();

    //public delegate void PacketHandler(int _clientID, Packet _packet);
    //private static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void StartServer(int _maxPlayers, int _port)
    {
        MaxPlayers = _maxPlayers;
        Port = _port;

        //InitializeServerData();
        for (int i = 0; i < MaxPlayers; i++)
        {
            Clients.Add(i, new NetworkClient(i));
        }

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        udpListener = new UdpClient(Port);
        udpListener.BeginReceive(new AsyncCallback(UDPReceiveCallback), null);
    }
    public static void SendUDP(IPEndPoint _endPoint, NetworkPacket _packet)
    {
        try
        {
            if (_endPoint == null)
                return;

            udpListener.BeginSend(_packet.ToArray(), _packet.Length, _endPoint, null, null);
        }
        catch(Exception _ex)
        {
            Console.WriteLine($"ZOINKS SCOOBS {_ex}");
        }
    }

    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        for (int i = 0; i < MaxPlayers; i++)
        {
            if(Clients[i].GetTCPSocket() == null)
            {
                Clients[i].ConnectTCP(client);
                return;
            }
        }
    }
    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0); //<-- this 0 is sus
            byte[] data = udpListener.EndReceive(_result, ref clientEndPoint);
            udpListener.BeginReceive(new AsyncCallback(UDPReceiveCallback), null);

            if (data.Length < 4)
                return;

            using(NetworkPacket _packet = new NetworkPacket(data))
            {
                int clientID = _packet.ReadInt();

                if (clientID == 0)
                    return;

                Clients[clientID].ValidateUDP(clientEndPoint, _packet);
            }
        }
        catch(Exception _ex)
        {
            Console.WriteLine(_ex);
        }
    }
}
