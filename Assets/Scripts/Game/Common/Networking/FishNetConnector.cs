using System;
using FishNet;
using FishNet.Managing.Client;
using FishNet.Object;
using FishNet.Transporting;
using LiteNetLib;
using TMPro;
using UnityEngine;

namespace Game.Common.Networking
{

    public class FishNetConnector : MonoBehaviour
    {
        
        
        public static void Host(ushort port)
        {
            var server = InstanceFinder.ServerManager;

            InstanceFinder.ServerManager.OnRemoteConnectionState += (connection, args) =>
            {
                switch (args.ConnectionState)
                {
                    case RemoteConnectionState.Stopped:
                        Debug.Log("Lost connection " + connection.ClientId);
                        break;
                    case RemoteConnectionState.Started:
                        Debug.Log("New connection " + connection.ClientId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            };
            
            server.StartConnection(port);
        }

        public static bool Connecting { get; private set; } 
        public static bool Connected { get; private set; }
        
        public static void Connect(string host, ushort port)
        {
            Connected = false;
            Connecting = true;
            var client = InstanceFinder.ClientManager;
            
            client.OnClientConnectionState += (args) =>
            {
                var connectionStr = host + ":" + port;
                switch (args.ConnectionState)
                {
                    case LocalConnectionState.Stopped:
                        Connected = false;
                        Debug.Log("Connection stopped " + connectionStr);
                        
                        break;
                    case LocalConnectionState.Starting:
                        Debug.Log("Connection starting " + connectionStr + " " + InstanceFinder.IsServer);
                        break;
                    case LocalConnectionState.Started:
                        Connected = true;
                        Connecting = false;
                        Debug.Log("Connection started " + connectionStr);
                        break;
                    case LocalConnectionState.Stopping:
                        Connected = false;
                        Debug.Log("Connection stopping " + connectionStr);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            };
            
            
            
            client.StartConnection(host, port);
        }
    }
}