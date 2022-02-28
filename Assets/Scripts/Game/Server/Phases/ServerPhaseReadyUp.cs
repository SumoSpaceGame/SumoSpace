using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;

namespace Game.Server.Phases
{
    public class ServerPhaseReadyUp : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;

        private List<uint> readyPlayers = new List<uint>();
        
        public ServerPhaseReadyUp(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
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
            if (data.Length != 1) return;

            if (data[0] == (byte)PhaseReadyUp.PLAYER_READY_FLAG)
            {
                readyPlayers.Add(info.SendingPlayer.NetworkId);
                _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_READY_UP, 
                    new [] {(byte)PhaseReadyUp.UPDATE_PLAYER_COUNT_FLAG, (byte)readyPlayers.Count, });
            }

            if (readyPlayers.Count == _phaseNetworkManager.gameMatchSettings.PlayerCount)
            {
                _phaseNetworkManager.ServerNextPhase();
            }
            
        }
    }
}