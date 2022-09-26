﻿using BeardedManStudios.Forge.Networking;
using Game.Common.Registry;

namespace Game.Common.Gameplay.Commands.Networkers
{
    public class ServerCommandNetworker : ICommandNetworker
    {
        private NetworkObject networker;
        private byte rpcMethodID;
        
        public ServerCommandNetworker(NetworkObject clientNetworker, byte RPC_METHOD_ID)
        {
            networker = clientNetworker;
            rpcMethodID = RPC_METHOD_ID;
        }

        public bool SendData(CommandPacketData data, int commandID, PlayerID shipID)
        {
            if (!networker.IsServer) return false;

            networker.SendRpc(rpcMethodID, Receivers.Others, commandID, data.GetBytes(), shipID.MatchID);

            return true;
        }
    }
}