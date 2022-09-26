using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Networking.Utility;
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
        private PlayerCounter playerCounter;
        public ServerPhaseSyncPlayerData(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            playerCounter = new PlayerCounter(gamePhaseNetworkManager.masterSettings.matchSettings.MaxPlayerCount);
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            Debug.Log("Sending clients player data");
            PhaseSyncPlayerData.Data data = new PhaseSyncPlayerData.Data();
            
            data.playerIDs = _gamePhaseNetworkManager.masterSettings.GetPlayerIDs();

            List<PlayerStaticData> staticDataList = new List<PlayerStaticData>();
            foreach (var playerID in data.playerIDs)
            {
                _gamePhaseNetworkManager.masterSettings.playerStaticDataRegistry.TryGet(playerID, out var staticData);
                staticDataList.Add(staticData);
            }

            data.staticData = staticDataList.ToArray();
            
            data.serverUpdateInterval = _gamePhaseNetworkManager.masterSettings.network.updateInterval;
            data.friendlyFire = _gamePhaseNetworkManager.masterSettings.matchSettings.FriendlyFire;
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
            
            PlayerID playerID;

            if (!_gamePhaseNetworkManager.masterSettings.playerIDRegistry.TryGetByNetworkID(info.SendingPlayer.NetworkId, out playerID))
            {
                Debug.LogWarning("Non-registered player tried to join the network! " + info.SendingPlayer.Ip);
                return;
            }
            
            playerCounter.Register(playerID);
            
            if (playerCounter.IsFull())
            {
                _gamePhaseNetworkManager.ServerNextPhase();
            }
            
        }
    }
}