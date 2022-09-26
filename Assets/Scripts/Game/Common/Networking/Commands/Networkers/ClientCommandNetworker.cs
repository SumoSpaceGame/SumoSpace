using BeardedManStudios.Forge.Networking;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Gameplay.Commands.Networkers
{
    public class ClientCommandNetworker : ICommandNetworker
    {
        private NetworkObject networker;
        private byte rpcMethodID;
        
        public ClientCommandNetworker(NetworkObject clientNetworker, byte RPC_METHOD_ID)
        {
            networker = clientNetworker;
            rpcMethodID = RPC_METHOD_ID;
        }
        
        public bool SendData(CommandPacketData data, int commandID, PlayerID shipID)
        {
            if (networker.IsServer || networker == null) return false;

            networker.SendRpc(rpcMethodID, Receivers.Server, commandID, data.GetBytes(), shipID.MatchID);

            return true;
        }
    }
}