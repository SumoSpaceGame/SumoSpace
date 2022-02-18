using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using Game.Client.SceneLoading;
using Game.Client.SceneLoading.SceneLoaderTasks;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine.SceneManagement;

namespace Game.Client.Phases
{
    public class ClientPhaseLoadMap : IGamePhase
    {

        private GamePhaseNetworkManager _phaseNetworkManager;
        private SceneLoader _sceneLoader;

        private ActivateEventTask _activateEventTask;
        
        public ClientPhaseLoadMap(GamePhaseNetworkManager phaseNetworkManager, SceneLoader sceneLoader)
        {
            _phaseNetworkManager = phaseNetworkManager;
            _sceneLoader = sceneLoader;
        }
        
        public void PhaseStart()
        {
            _activateEventTask = new ActivateEventTask("Waiting for players..");

            _activateEventTask.OnActivateEvent += OnWaitForPlayers;
            
            _sceneLoader.FinishLoadingSceneEvent += OnSceneLoaded;
            _sceneLoader.Load(new List<ISceneLoaderTask>()
            {
                new LoadSceneTask("TestMap"),
                _activateEventTask
            });
            
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
            _sceneLoader.FinishLoadingSceneEvent += OnSceneLoaded;
            _activateEventTask = null;
        }

        private void OnSceneLoaded()
        {
            _sceneLoader.FinishLoadingSceneEvent -= OnSceneLoaded;
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOAD_MAP, new byte[]{});
        }

        private void OnWaitForPlayers()
        {
            _activateEventTask.OnActivateEvent -= OnWaitForPlayers;
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_LOAD_MAP, new byte[]{});
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            if (info.SendingPlayer.IsHost)
            {
                if (data[0] == 1)
                {
                    _activateEventTask.FinishActivateEvent();
                }
            }
            
        }
    }
}