using Game.Common.Networking;
using Game.Common.Registry;

namespace Game.Common.Gameplay.Commands.Networkers
{
    public class ServerCommandNetworker : ICommandNetworker
    {
        private InputLayerNetworkManager networker;
        
        public ServerCommandNetworker(InputLayerNetworkManager clientNetworker)
        {
            networker = clientNetworker;
        }

        public bool SendData(CommandPacketData data, CommandType commandID, PlayerID shipID)
        {
            if (!networker.IsServer) return false;
            
            networker.ClientCommandUpdate(commandID, data.GetBytes(), shipID.MatchID);

            return true;
        }
    }
}