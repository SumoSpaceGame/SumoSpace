using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseSyncPlayerData : IGamePhase
    {
        private GamePhaseNetworkManager _gamePhaseNetworkManager;

        private Dictionary<uint, PlayerID> finishedSynced = new Dictionary<uint, PlayerID>();
        public ServerPhaseSyncPlayerData(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            Debug.Log("Sending clients player data");
            PhaseSyncPlayerData.Data data = new PhaseSyncPlayerData.Data();
            data.playerIDs = _gamePhaseNetworkManager.masterSettings.GetPlayerIDs();
            
            _gamePhaseNetworkManager.SendPhaseUpdate(Phase.MATCH_SYNC_PLAYER_DATA, PhaseSyncPlayerData.Serialized(data));
        }

        public void PhaseUpdate()
        {
            
        }

        public void PhaseCleanUp()
        {
            
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            if (finishedSynced.ContainsKey(info.SendingPlayer.NetworkId))
            {
                Debug.LogWarning("Double update received for player sync, what happened?");
                return;
            }
            
            finishedSynced.Add(info.SendingPlayer.NetworkId, 
                _gamePhaseNetworkManager.masterSettings.playerIDRegistry.Get(info.SendingPlayer.NetworkId));
            
            Debug.Log($"Checking - {finishedSynced.Count} == {_gamePhaseNetworkManager.masterSettings.matchSettings.PlayerCount}");
            
            if (finishedSynced.Count == _gamePhaseNetworkManager.masterSettings.matchSettings.PlayerCount)
            {
                _gamePhaseNetworkManager.ServerNextPhase();
            }
            
        }
    }
}