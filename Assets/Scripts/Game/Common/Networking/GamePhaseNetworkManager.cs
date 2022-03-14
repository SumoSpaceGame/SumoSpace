using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Game.Common.Phases;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class GamePhaseNetworkManager : GamePhaseBehavior, IGamePersistantInstance
    {

        public MasterSettings masterSettings;
        public GameMatchSettings gameMatchSettings;
        
        /// <summary>
        /// Data to store while waiting to push into phase from update RPC.
        /// </summary>
        private struct UpdateQueueData
        {
            public UpdateQueueData(RPCInfo info, byte[] data)
            {
                Info = info;
                Data = data;
            } 
            
            public RPCInfo Info;
            public byte[] Data;
        }

        private GamePhaseManager _gamePhaseManager = new GamePhaseManager();

        public Phase CurrentPhase => _gamePhaseManager.CurrentPhase;

        private bool finishedNetworkStart = false;
        
        private Dictionary<Phase, IGamePhase> gamePhases =
            new Dictionary<Phase, IGamePhase>();

        private Dictionary<Phase, Queue<UpdateQueueData>> updateQueueData =
            new Dictionary<Phase, Queue<UpdateQueueData>>();
        
        // Unity functions
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            for (int i = 0; i <= GamePhaseManager.PHASE_MAX_ID; i++)
            {
                updateQueueData.Add((Phase) i, new Queue<UpdateQueueData>());
            }
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            //Initiate queue data

            
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });

            if (networkObject.IsServer)
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
            
            if (networkObject.IsServer)
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
                curPhase.OnUpdateReceived(updateData.Info, updateData.Data);
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
        public void SendPhaseUpdate(Phase phase, byte[] data)
        {
            var phaseID = (int) phase;   
            networkObject.SendRpc(RPC_UPDATE_PHASE, 
                this.networkObject.IsServer ? Receivers.Others : Receivers.Server,
                phaseID, data);
        }
        
        /// <summary>
        /// Used by other classes to send a unreliable update
        /// </summary>
        /// <param name="phase"></param>
        /// <param name="data"></param>
        public void SendUnreliablePhaseUpdate(Phase phase, byte[] data)
        {
            var phaseID = (int) phase;   
            networkObject.SendRpc(RPC_UPDATE_PHASE, 
                this.networkObject.IsServer ? Receivers.Others : Receivers.Server,
                phaseID, data);
        }
        
        
        /// <summary>
        /// Processes update RPC. Checks if data is good, if so add it to a queue for the phase to run after update.
        /// </summary>
        /// <param name="args"></param>
        public override void UpdatePhase(RpcArgs args)
        {
            if ( !this.networkObject.IsServer && !args.Info.SendingPlayer.IsHost)
            {
                return;
            }
            
            var phaseID = args.GetAt<int>(0);
            var updateData = args.GetAt<byte[]>(1);

            // Make sure phaseID is within the range
            if (!_gamePhaseManager.WithinPhaseRange(phaseID))
            {
                return;
            }

            var updatePhase = (Phase) phaseID;

            
            MainThreadManager.Run(() =>
            {
                
                // If an update is sent before this has been finished
                if (updatePhase != _gamePhaseManager.CurrentPhase)
                {
                    // TODO: Record this as a suspicious activity
                        Debug.LogWarning($"Update phase received out of order! {updatePhase.ToString()}");
                }
                updateQueueData[updatePhase].Enqueue(new UpdateQueueData(args.Info, updateData));
            });
        }

        /// <summary>
        /// Network input. Process phase switch.
        /// </summary>
        public override void SwitchPhase(RpcArgs args)
        {
            if (!args.Info.SendingPlayer.IsHost)
            {
                return;
            }

            var nextPhase = (Phase) args.GetAt<int>(0);
            
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