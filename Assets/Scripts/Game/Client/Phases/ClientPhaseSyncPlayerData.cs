using BeardedManStudios.Forge.Networking;
using FishNet.Connection;
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

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            
            var syncData = PhaseSyncPlayerData.Deserialize(data);

            if (!syncData.valid)
            {
                Debug.LogError("Fatal - Failed to sync player data");
                return;
            }
            
            for(int i = 0 ; i < syncData.playerIDs.Length; i++)
            {
                var playerID = syncData.playerIDs[i];
                Debug.Log($"Synced player {playerID}");
                var id = _phaseNetworkManager.masterSettings.RegisterPlayer(playerID.NetworkID, playerID.MatchID, playerID.ClientID, syncData.staticData[i]);
            }
            Debug.Log("Synced played data");

            // TODO: Sync in a better location
            _phaseNetworkManager.masterSettings.matchSettings.FriendlyFire = syncData.friendlyFire;
            
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_SYNC_PLAYER_DATA, new byte[1]);
        }
    }
}