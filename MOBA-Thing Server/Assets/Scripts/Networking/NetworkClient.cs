using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class NetworkClient
{
    public const int DATA_BUFFER_SIZE = 4096;

    public int ClientID;
    public int EntityID;

    private TCP tcp;
    private UDP udp;

    public NetworkClient(int _clientID)
    {
        ClientID = _clientID;
        tcp = new TCP(ClientID);
        udp = new UDP(ClientID);
    }

    public TcpClient GetTCPSocket()
    {
        return tcp.socket;
    }
    public void ConnectTCP(TcpClient _client)
    {
        tcp.Connect(_client);
    }
    public void ValidateUDP(IPEndPoint _endPoint, NetworkPacket _packet)
    {
        if (udp.EndPoint == null)
        {
            udp.Connect(_endPoint);
            return;
        }

        if(udp.EndPoint.ToString() == _endPoint.ToString())
            udp.HandleData(_packet);
    }

    public void SendDataByUDP(NetworkPacket _packet)
    {
        udp.SendData(_packet);
    }
    public void SendDataByTCP(NetworkPacket _packet)
    {
        tcp.SendData(_packet);
    }

    private class TCP
    {
        public TcpClient socket;

        private readonly int id;
        private NetworkStream netStream;
        private NetworkPacket receivedData;
        private byte[] dataBuffer;

        public TCP(int _id)
        {
            id = _id;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.SendBufferSize = socket.ReceiveBufferSize = DATA_BUFFER_SIZE;

            netStream = socket.GetStream();

            receivedData = new NetworkPacket();
            dataBuffer = new byte[DATA_BUFFER_SIZE];

            netStream.BeginRead(dataBuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);

            ServerSend.Welcome(id, "yeet son");
        }

        public void SendData(NetworkPacket _packet)
        {
            try
            {
                if (socket != null)
                    netStream.BeginWrite(_packet.ToArray(), 0, _packet.Length, null, null);
            }
            catch (Exception _ex)
            {
                Console.WriteLine($"FUCK IT BROKE DUDE: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int byteLength = netStream.EndRead(_result);
                if(byteLength <= 0)
                {
                    ServerBody.Clients[id].Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(dataBuffer, data, byteLength);

                receivedData.Reset(HandleData(data));
                netStream.BeginRead(dataBuffer, 0, DATA_BUFFER_SIZE, ReceiveCallback, null);
            }
            catch(Exception _ex)
            {
                Console.WriteLine($"Bruh fuckin everything is going wrong rn: {_ex}");
                ServerBody.Clients[id].Disconnect();
            }
        }

        private bool HandleData(byte[] _data)
        {
            int packetLength = 0;

            receivedData.SetBytes(_data);

            if(receivedData.UnreadLength >= 4)
            {
                packetLength = receivedData.ReadInt();
                if (packetLength <= 0)
                    return true;
            }

            while(packetLength > 0 && packetLength <= receivedData.UnreadLength)
            {
                byte[] packetBytes = receivedData.ReadBytes(packetLength);

                ThreadManager.ExecuteOnMain(() =>
                {
                    using (Packet _packet = new Packet(packetBytes))
                    {
                        int packetID = _packet.ReadInt();
                        //ServerBody.packetHandlers[packetID](id, _packet); <-- this bit is no bueno
                    }
                });

                packetLength = 0;
                if(receivedData.UnreadLength >= 4)
                {
                    packetLength = receivedData.ReadInt();
                    if(packetLength <= 0)
                        return true;
                }
            }

            if (packetLength <= 1)
                return true;
            return false;
        }

        public void Disconnect()
        {
            socket.Close();
            netStream = null;
            receivedData = null;
            dataBuffer = null;
            socket = null;
        }
    }
    private class UDP
    {
        public IPEndPoint EndPoint;

        private readonly int id;

        public UDP(int _id)
        {
            id = _id;
        }

        public void Connect(IPEndPoint _endPoint)
        {
            EndPoint = _endPoint;
        }

        public void SendData(NetworkPacket _packet)
        {
            ServerBody.SendUDP(EndPoint, _packet);
        }

        public void HandleData(NetworkPacket _packet)
        {
            int packetLength = _packet.ReadInt();
            byte[] packetBytes = _packet.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMain(() =>
            {
                using (Packet _packet = new Packet(packetBytes))
                {
                    int packetID = _packet.ReadInt();
                    ServerBody.packetHandlers[packetID](id, _packet); //<-- also no bueno
                }
            });
        }

        public void Disconnect()
        {
            EndPoint = null;
        }
    }

    private void Disconnect()
    {
        tcp.Disconnect();
        udp.Disconnect();
    }
}
