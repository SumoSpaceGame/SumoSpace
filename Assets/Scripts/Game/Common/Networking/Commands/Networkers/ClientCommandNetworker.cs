using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Common.Gameplay.Commands.Networkers
{
    public class ClientCommandNetworker : ICommandNetworker
    {
        private InputLayerNetworkManager networker;
        
        public ClientCommandNetworker(InputLayerNetworkManager clientNetworker)
        {
            networker = clientNetworker;
        }
        
        public bool SendData(CommandPacketData data, CommandType commandID, PlayerID shipID)
        {
            if (networker.IsServer || networker == null) return false;

            networker.ServerCommandUpdate(commandID, data.GetBytes(), shipID.MatchID);

            return true;
        }
    }
}