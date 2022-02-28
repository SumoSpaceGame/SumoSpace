using BeardedManStudios.Forge.Networking;
using Game.Common.Gameplay.Ship;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.Registry;

namespace Game.Client.Phases
{
    public class ClientPhaseSyncLoadout : IGamePhase
    {
        private GamePhaseNetworkManager _gamePhaseNetworkManager;
        private PlayerGameDataRegistry _playerGameDataRegistry;
        private PlayerIDRegistry _playerIDRegistry;
        
        public ClientPhaseSyncLoadout(GamePhaseNetworkManager gamePhaseNetworkManager, PlayerGameDataRegistry gameDataRegistry, PlayerIDRegistry playerIDRegistry)
        {
            _gamePhaseNetworkManager = gamePhaseNetworkManager;
            _playerGameDataRegistry = gameDataRegistry;
            _playerIDRegistry = playerIDRegistry;
        }

        public void PhaseStart()
        {
            //Start timer dependent on time length
        }

        public void PhaseUpdate()
        {
        }


        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            var syncData = PhaseSyncLoadout.Decode(data);

            for (int i = 0; i < syncData.PlayerCount; i++)
            {
                
                //var shipInfo = ShipCreationData.Create(syncData.PlayerSelections[i]);
                
                //var gameData = _gamePhaseNetworkManager.masterSettings.playerGameDataRegistry.

            }
            //Update ui to show syncData
            
        }
    }
}