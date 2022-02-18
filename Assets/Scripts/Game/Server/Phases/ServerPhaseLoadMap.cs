using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Client.SceneLoading;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Server.Phases
{
    public class ServerPhaseLoadMap : IGamePhase
    {
        
        private GamePhaseNetworkManager _phaseNetworkManager;
        private AgentNetworkManager _agentNetworkManager;

        private List<uint> loadedPlayers = new List<uint>();

        private bool sceneLoaded = false;

        private SceneLoader _sceneLoader;
        
        public ServerPhaseLoadMap(GamePhaseNetworkManager phaseNetworkManager, SceneLoader sceneLoader)
        {
            _phaseNetworkManager = phaseNetworkManager;
            _sceneLoader = sceneLoader;
        }
        
        public void PhaseStart()
        {
            _agentNetworkManager = MainPersistantInstances.Get<AgentNetworkManager>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(3);
        }

        public void PhaseUpdate()
        {
            if (sceneLoaded && loadedPlayers.Count == _phaseNetworkManager.gameMatchSettings.PlayerCount)
            {
                foreach (uint player in loadedPlayers)
                {
                    Debug.Log("Spawning " + player);
                    _agentNetworkManager.SpawnShip(player);
                }
                
                _phaseNetworkManager.ServerNextPhase();

                sceneLoaded = false;
            }
        }

        public void PhaseCleanUp()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            sceneLoaded = false;
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            sceneLoaded = true;
        }

        public void OnUpdateReceived(RPCInfo info, byte[] data)
        {
            
            if (loadedPlayers.Contains(info.SendingPlayer.NetworkId))
            {
                Debug.LogError("Duplicate update received for temp load map solution");
                return;
            }
            
            
            loadedPlayers.Add(info.SendingPlayer.NetworkId);

        }
    }
}