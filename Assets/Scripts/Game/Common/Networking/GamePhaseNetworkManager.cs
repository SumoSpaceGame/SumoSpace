using System.Collections.Generic;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using Game.Common.Instances;
using Game.Common.Phases;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class GamePhaseNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {

        public MasterSettings masterSettings;
        public GameMatchSettings gameMatchSettings;
        
        /// <summary>
        /// Data to store while waiting to push into phase from update RPC.
        /// </summary>
        private struct UpdateQueueData
        {
            public UpdateQueueData(NetworkConnection conn, byte[] data)
            {
                Conn = conn;
                Data = data;
            } 
            
            public NetworkConnection Conn;
            public byte[] Data;
        }

        private GamePhaseManager _gamePhaseManager = new GamePhaseManager();

        public Phase CurrentPhase => _gamePhaseManager.CurrentPhase;

        private bool finishedNetworkStart = false;
        
        private Dictionary<Phase, IGamePhase> gamePhases =
            new Dictionary<Phase, IGamePhase>();

        private Dictionary<Phase, Queue<UpdateQueueData>> updateQueueData =
            new Dictionary<Phase, Queue<UpdateQueueData>>();
        

        private void Start()
        {
            for (int i = 0; i <= GamePhaseManager.PHASE_MAX_ID; i++)
            {
                updateQueueData.Add((Phase) i, new Queue<UpdateQueueData>());
            }
        }

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            
            MainPersistantInstances.TryAdd(this);

            if (InstanceFinder.IsServer)
            {
                ServerAddPhases();
            }
            else
            {
                ClientAddPhases();
            }
            
            _gamePhaseManager.OnPhaseUpdate += UpdatePhases;
            _gamePhaseManager.OnPhaseSwitch += OnPhaseSwitch;
            
            _gamePhaseManager.SwitchPhase(Phase.MATCH_CONNECT);

            finishedNetworkStart = true;
            
            
        }

        private void Update()
        {
            if (!finishedNetworkStart) return;
            
            if (InstanceFinder.IsServer)
            {
                ServerUpdate();
            }
            else
            {
                ClientUpdate();
            }
            _gamePhaseManager.UpdatePhase();

            // Check if theres an data to push through update
            // Applies after update to make sure update is ran atleast once.
            var dataQueue = updateQueueData[_gamePhaseManager.CurrentPhase];
            var curPhase = gamePhases[_gamePhaseManager.CurrentPhase];
            
            while (dataQueue.Count > 0)
            {
                var updateData = dataQueue.Dequeue();
                curPhase.OnUpdateReceived(updateData.Conn, updateData.Data);
            }
        }

        private void UpdatePhases()
        {
            var curPhase = gamePhases[_gamePhaseManager.CurrentPhase];
            curPhase.PhaseUpdate();
        }

        private void OnDestroy()
        {
            GetCurrentPhase().PhaseCleanUp();
            MainPersistantInstances.Remove<GamePhaseNetworkManager>();
            Destroy(this.gameObject);
        }

        // RPCs

        /// <summary>
        /// Used by other classes to send a update to the phases of the clients or server.
        /// </summary>
        /// <param name="phase"></param>
        /// <param name="data"></param>
        public void SendPhaseUpdate(Phase phase, byte[] data, Channel channel = Channel.Reliable)
        {
            var phaseID = (int) phase;   
            if (InstanceFinder.IsServer)
            {
                SendClientPhaseUpdate(phase, data, channel);
            }
            else
            {
                SendServerPhaseUpdate(phase, data, channel);
            }
            
        }

        [ServerRpc(RequireOwnership = false)]
        public void SendServerPhaseUpdate(Phase phase, byte[] data, Channel channel = Channel.Reliable, NetworkConnection conn = null)
        {
            
            var phaseID = (int) phase;
            var updateData = data;

            // Make sure phaseID is within the range
            if (!_gamePhaseManager.WithinPhaseRange(phaseID))
            {
                return;
            }

            var updatePhase = (Phase) phaseID;

            // If an update is sent before this has been finished
            if (updatePhase != _gamePhaseManager.CurrentPhase)
            {
                // TODO: Record this as a suspicious activity
                Debug.LogWarning($"Update phase received out of order! {updatePhase.ToString()}");
            }
            updateQueueData[updatePhase].Enqueue(new UpdateQueueData(conn, updateData));
        }
        
        [ObserversRpc]
        public void SendClientPhaseUpdate(Phase phase, byte[] data, Channel channel = Channel.Reliable)
        {
            
            var phaseID = (int) phase;
            var updateData = data;

            // Make sure phaseID is within the range
            if (!_gamePhaseManager.WithinPhaseRange(phaseID))
            {
                return;
            }

            var updatePhase = (Phase) phaseID;

            // If an update is sent before this has been finished
            if (updatePhase != _gamePhaseManager.CurrentPhase)
            {
                // TODO: Record this as a suspicious activity
                Debug.LogWarning($"Update phase received out of order! {updatePhase.ToString()}");
            }
            updateQueueData[updatePhase].Enqueue(new UpdateQueueData(null, updateData));
        }

        /// <summary>
        /// Used by other classes to send a unreliable update
        /// </summary>
        /// <param name="phase"></param>
        /// <param name="data"></param>
        [ServerRpc(RequireOwnership = false)]
        public void SendUnreliablePhaseUpdate(Phase phase, byte[] data)
        {
            SendPhaseUpdate(phase, data, Channel.Unreliable);
        }
        
        

        [ObserversRpc]
        public void SwitchPhase(Phase nextPhase)
        {
            
            _gamePhaseManager.SwitchPhase(nextPhase);
        }

        private void OnPhaseSwitch(Phase lastPhase, Phase nextPhase)
        {
            
            var lastPhaseObj = gamePhases[lastPhase];
            var nextPhaseObj = gamePhases[nextPhase];
            lastPhaseObj.PhaseCleanUp();
            nextPhaseObj.PhaseStart();
        }
        // Helpers

        private IGamePhase GetCurrentPhase()
        {
            return this.gamePhases[this._gamePhaseManager.CurrentPhase];
        }

        // Partial methods

        partial void ClientAddPhases();
        partial void ServerAddPhases();
        
        partial void ClientUpdate();
        partial void ServerUpdate();
    }
}