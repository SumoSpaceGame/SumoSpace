using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Client.SceneLoading;
using Game.Client.SceneLoading.SceneLoaderTasks;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine;
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
            
            MainThreadManager.Run(() =>
            {
                Debug.Log("Starting loading task");
                _sceneLoader.Load(new List<ISceneLoaderTask>()
                {
                    new LoadSceneTask(_phaseNetworkManager.gameMatchSettings.SelectedMap, _phaseNetworkManager.StartCoroutine),
                    _activateEventTask
                });
            });
            
        }

        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
            MainThreadManager.Run(() =>
            {
                _activateEventTask.FinishActivateEvent();
            });
            _sceneLoader.FinishLoadingSceneEvent -= OnSceneLoaded;
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
                    MainThreadManager.Run(() =>
                    {
                        _activateEventTask.FinishActivateEvent();
                    });
                }
            }
            
        }
    }
}