using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseSyncLoadout : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;
        public ServerPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _phaseNetworkManager = gamePhaseNetworkManager;
        }

        public void PhaseStart()
        {
            
        }

        public void PhaseUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void PhaseCleanUp()
        {
            throw new System.NotImplementedException();
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            Debug.Log(
                "Received data for sync loadout on server. This should not happen. Is someone messing with the game?");
        }
    }
}