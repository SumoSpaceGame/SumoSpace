using System.Collections.Generic;
using FishNet.Connection;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;

namespace Game.Server.Phases
{
    public class ServerPhaseReadyUp : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;

        private List<int> readyPlayers = new List<int>();
        
        public ServerPhaseReadyUp(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
        }

        public void PhaseStart()
        {
            _phaseNetworkManager.gameMatchSettings.MatchStarted = true;
            _phaseNetworkManager.gameMatchSettings.ServerRestartOnLeave = true; 
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            if (data.Length != 1) return;

            if (data[0] == (byte)PhaseReadyUp.PLAYER_READY_FLAG)
            {
                readyPlayers.Add(conn.ClientId);
                _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_READY_UP, 
                    new [] {(byte)PhaseReadyUp.UPDATE_PLAYER_COUNT_FLAG, (byte)readyPlayers.Count, });
            }

            if (readyPlayers.Count == _phaseNetworkManager.gameMatchSettings.MaxPlayerCount)
            {
                _phaseNetworkManager.ServerNextPhase();
            }
            
        }
    }
}