
using System;
using System.Linq;
using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Map;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client.Phases
{
    public class ClientPhaseStartMatch : IGamePhase
    {
        public GamePhaseNetworkManager _gamePhaseNetworkManager;

        public ClientPhaseStartMatch(GamePhaseNetworkManager _gamePhaseNetworkManager)
        {
            this._gamePhaseNetworkManager = _gamePhaseNetworkManager;
        }
        
        public void PhaseStart()
        {
            
        }

        private bool sentReady = false;

        public void PhaseUpdate()
        {
            if (!sentReady && SceneManager.GetActiveScene().name == _gamePhaseNetworkManager.masterSettings.matchSettings.SelectedMapItem.sceneName)
            {
                sentReady = true;
                _gamePhaseNetworkManager.SendPhaseUpdate(Phase.MATCH_START_COUNTDOWN, new byte[] {1});
            }
        }

        public void PhaseCleanUp()
        {
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            if (data.Length == 0)
            {
                Debug.LogError("Phase update received invalid input");
            }

            switch (data[0])
            {
                case 0:
                    // Activate map
                    uint timerID = BitConverter.ToUInt32(data.Skip(1).ToArray());
                    
                    MainInstances.Get<GameMapManager>().ActivateMap(timerID);
                    
                    break;
            }
        }
    }
}