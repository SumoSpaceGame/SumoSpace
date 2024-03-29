﻿using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Client.Phases
{
    public class ClientPhaseLobby : IGamePhase
    {

        private GamePhaseNetworkManager _phaseNetworkManager;
        private MasterUIController _masterUIController;

        private int selectedCharacter = -1;

        public ClientPhaseLobby(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
        }
        public void PhaseStart()
        {
            SceneManager.LoadScene("LobbyScene");
            _masterUIController = MainPersistantInstances.Get<MasterUIController>();
            
            _masterUIController.selectCharacter1.onClick.AddListener(() =>
            {
                SelectCharacter(0);
            });
            _masterUIController.selectCharacter2.onClick.AddListener(() =>
            {
                SelectCharacter(1);
            });
            _masterUIController.selectCharacter3.onClick.AddListener(() =>
            {
                SelectCharacter(2);
            });
            
            _masterUIController.lockedInButton.onClick.AddListener(() =>
            {
                LockInCharacter();
            });
            
            _masterUIController.ActivateLobby();

        }

        private void SelectCharacter(int num)
        {
            selectedCharacter = num;
            _phaseNetworkManager.SendUnreliablePhaseUpdate(Phase.MATCH_LOBBY, 
                new []{(byte) PhaseLobby.PLAYER_SELECT_FLAG, (byte) selectedCharacter});
        }

        private void LockInCharacter()
        {
            if (selectedCharacter == -1)
            {
                Debug.Log("None selected, defaulting to 0");
                SelectCharacter(0);
            }
            
            _phaseNetworkManager.SendUnreliablePhaseUpdate(Phase.MATCH_LOBBY, 
                new []{(byte) PhaseLobby.PLAYER_LOCKED_FLAG, (byte)selectedCharacter});
        }
        
        public void PhaseUpdate()
        {
        }

        public void PhaseCleanUp()
        {
            _masterUIController.StopLobby();
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            if (data.Length == 0) return;

            Debug.Log("Received data " + data.ToString() + " " + data.Length + " " + data[0] + " " + data[1] + " " + _phaseNetworkManager.masterSettings.matchSettings.ClientMatchID);
            switch (data.Length)
            {
                case 1:
                    break;
                case 3:
                    
                    if (data[0] == PhaseLobby.PLAYER_SELECT_FLAG)
                    {
                        //Debug.Log(data[1] + " selected " +  data[2]);
                        
                        if (data[1] == _phaseNetworkManager.masterSettings.matchSettings.ClientMatchID)
                        {
                            Debug.Log("Server said I am this character");
                        }
                        
                        // Save their data
                        var playerLockedIn = _phaseNetworkManager.masterSettings.playerIDRegistry.GetByMatchID(data[1]);
                        if (_phaseNetworkManager.masterSettings.playerGameDataRegistry.TryGet(playerLockedIn, out var gameData))
                        {
                            gameData.shipCreationData.shipType = data[2];
                        }
                    }
                    break;
                case 4:

                    if (data[0] == PhaseLobby.PLAYER_LOCKED_FLAG)
                    {
                        //Debug.Log(data[1] + " locked in " +  data[3]);

                        if (data[1] == _phaseNetworkManager.gameMatchSettings.ClientMatchID)
                        {
                            Debug.Log("Server confirmed I'm locked and loaded");
                            _masterUIController.LockLobby();
                        }
                        
                        
                        // Save their locked data
                        var playerLockedIn = _phaseNetworkManager.masterSettings.playerIDRegistry.GetByMatchID(data[1]);
                        if (_phaseNetworkManager.masterSettings.playerGameDataRegistry.TryGet(playerLockedIn, out var gameData))
                        {
                            gameData.shipCreationData.shipType = data[3];
                            gameData.shipCreationData.playerLockedIn = true;
                        }
                    }
                    
                    
                    break;
            }
        }
    }
}