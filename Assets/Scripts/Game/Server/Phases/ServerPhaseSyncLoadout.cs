using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;
using UnityEngine;

namespace Game.Server.Phases
{
    public class ServerPhaseSyncLoadout : IGamePhase
    {
        private GamePhaseNetworkManager _phaseNetworkManager;

        private Dictionary<uint, PlayerID> _syncedPlayer = new Dictionary<uint, PlayerID>();
        public ServerPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager)
        {
            _phaseNetworkManager = gamePhaseNetworkManager;
        }

        public void PhaseStart()
        {
            PhaseSyncLoadout.Data data = new PhaseSyncLoadout.Data();

            var playerList = new List<ushort>();
            var selectionList = new List<int>();

            var players = _phaseNetworkManager.masterSettings.GetPlayerIDs();

            foreach (var id in players)
            {
                playerList.Add(id.MatchID);
                
                selectionList.Add(_phaseNetworkManager.masterSettings.playerGameDataRegistry.Get(id).shipCreationData.shipType);
            }

            data.PlayerSelections = selectionList.ToArray();
            data.PlayerIDs = playerList.ToArray();
            
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_SYNC_LOAD_OUTS, PhaseSyncLoadout.Serialized(data));
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
            if (!_phaseNetworkManager.masterSettings.playerIDRegistry.TryGetByNetworkID(info.SendingPlayer.NetworkId,
                    out playerID))
            {
                Debug.LogWarning("WARNING: Phase data recieved from client that is not registered? Spectator?");
                return;
            }
            
            _syncedPlayer.Add(info.SendingPlayer.NetworkId, playerID);

            if (_syncedPlayer.Count == _phaseNetworkManager.gameMatchSettings.MaxPlayerCount)
            {
                _phaseNetworkManager.ServerNextPhase();
            }

        }
    }
}