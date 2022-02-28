using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Client.Phases
{
    public class ClientPhaseSyncPlayerData : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;
        public ClientPhaseSyncPlayerData(GamePhaseNetworkManager _phaseNetworkManager)
        {
            this._phaseNetworkManager = _phaseNetworkManager;
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
            
            var syncData = PhaseSyncPlayerData.Deserialize(data);

            if (!syncData.valid)
            {
                Debug.LogError("Fatal - Failed to sync player data");
                return;
            }
            
            foreach(var playerID in syncData.playerIDs)
            {
                _phaseNetworkManager.masterSettings.playerIDRegistry.RegisterPlayer(playerID.ID, playerID.MatchID, playerID.ClientID);
                var savedID = _phaseNetworkManager.masterSettings.playerIDRegistry.Get(playerID.ID);


                _phaseNetworkManager.masterSettings.playerStaticDataRegistry.Add(savedID, new PlayerStaticData() { });
                _phaseNetworkManager.masterSettings.playerGameDataRegistry.Add(savedID, new PlayerGameData() { });
            }
            Debug.Log("Synced played data");
            
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_SYNC_PLAYER_DATA, new byte[1]);
        }
    }
}