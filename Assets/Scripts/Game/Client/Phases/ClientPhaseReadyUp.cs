using FishNet.Connection;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Phases;
using Game.Common.Phases.PhaseData;
using Game.Common.UI;

namespace Game.Client.Phases
{
    public class ClientPhaseReadyUp : IGamePhase
    {

        private GamePhaseNetworkManager _phaseNetworkManager;
        private MasterUIController _masterUIController;
        private bool readyUp = false;
        private bool waitForServer = false;

        private int currentReadyCount = 0;
        
        public ClientPhaseReadyUp(GamePhaseNetworkManager phaseNetworkManager)
        {
            _phaseNetworkManager = phaseNetworkManager;
            _masterUIController = MainPersistantInstances.Get<MasterUIController>();
        }

        public void PhaseStart()
        {
            _phaseNetworkManager.gameMatchSettings.MatchStarted = true; 
            _phaseNetworkManager.gameMatchSettings.ServerRestartOnLeave = true; 
            _masterUIController.ActivateReady();
            _masterUIController.ReadyButton.onClick.AddListener(onReadyClicked);
            _masterUIController.ReadyUpText.text = "Ready up";
        }


        public void PhaseUpdate()
        {
            if (waitForServer)
            {
                _masterUIController.ReadyUpText.text = $"Waiting for players.. ({currentReadyCount})";
                return;
            }
            
            if (readyUp)
            {
                _masterUIController.ReadyUpText.text = "Waiting for players..";
                waitForServer = true;
                SendServerReady();
            }
        }

        private void SendServerReady()
        {
            _phaseNetworkManager.SendPhaseUpdate(Phase.MATCH_READY_UP, 
                new []{(byte)PhaseReadyUp.PLAYER_READY_FLAG});
        }
        
        public void PhaseCleanUp()
        {
            _masterUIController.ReadyButton.onClick.RemoveListener(onReadyClicked);
            _masterUIController.StopReady();
            readyUp = false;
            waitForServer = false;
        }

        public void OnUpdateReceived(NetworkConnection conn, byte[] data)
        {
            if (data.Length >= 1)
            {
                if (data[0] == PhaseReadyUp.UPDATE_PLAYER_COUNT_FLAG)
                {
                    currentReadyCount = data[1];
                }
            }
        }
        
        
        private void onReadyClicked()
        {
            readyUp = true;
        }
    }
}