using System;
using FishNet;
using FishNet.Object;
using FishNet.Transporting;
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

        private static bool Connecting = false;
        public static void Connect(string host, ushort port)
        { 
            Connecting = true;
            var client = InstanceFinder.ClientManager;
            
            client.OnClientConnectionState += (args) =>
            {
                var connectionStr = host + ":" + port;
                switch (args.ConnectionState)
                {
                    case LocalConnectionState.Stopped:
                        Connecting = false;
                        Debug.Log("Connection stopped " + connectionStr);
                        break;
                    case LocalConnectionState.Starting:
                        Debug.Log("Connection starting " + connectionStr + " " + InstanceFinder.IsServer);
                        break;
                    case LocalConnectionState.Started:
                        Debug.Log("Connection started " + connectionStr);
                        break;
                    case LocalConnectionState.Stopping:
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