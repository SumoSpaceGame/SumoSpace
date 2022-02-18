using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;

namespace Game.Server.Phases
{
    public class ServerPhaseSyncPlayerData : IGamePhase
    {
        private GamePhaseNetworkManager _gamePhaseNetworkManager;
        
        public ServerPhaseSyncPlayerData(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            
        }

        public void PhaseUpdate()
        {
            
        }

        public void PhaseCleanUp()
        {
            
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            
        }
    }
}