using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking.Misc
{
    public class MatchNetworkTimerManager : MatchTimerBehavior, IGamePersistantInstance
    {
        private Dictionary<uint, MatchNetworkTimer> _timers = new Dictionary<uint, MatchNetworkTimer>();

        private uint _timerCounter = 0;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            MainPersistantInstances.Add(this);
        }
        
        protected override void NetworkStart()
        {
            base.NetworkStart();
        }

        /// <summary>
        /// Updates the timers, and when they are destroyed it will destroy them.
        /// </summary>
        private void Update()
        {
            List<uint> destroyedIDs = new List<uint>();
            
            foreach (var timer in _timers.Values)
            {
                if (timer.IsDestroyed)
                {
                    destroyedIDs.Add(timer.ID);
                    continue;
                }
                
                timer.Tick();
            }

            foreach (var timerID in destroyedIDs)
            {
                _timers.Remove(timerID);
            }
        }

        /// <summary>
        /// Creates a timer, server only
        /// </summary>
        /// <param name="timer">Timer object</param>
        /// <returns>Timer Object, it also has an ID</returns>
        public MatchNetworkTimer CreateTimer()
        {
            if (!networkObject.IsServer)
            {
                Debug.LogError("Can not create timer on the client, has to be on the server");
                return null;
            }
            
            var timer = CreateNewTimer();
            
            _timers.Add(timer.ID, timer);
            
            networkObject.SendRpc(RPC_CREATE_CLIENT_TIMER_HANDLER, Receivers.Others, timer.ID);

            return timer;
        }

        /// <summary>
        /// Grabs the timer you need
        /// </summary>
        /// <param name="id"></param>
        /// <param name="timer"></param>
        /// <returns></returns>
        public bool GetTimer(uint id, out MatchNetworkTimer timer)
        {
            return _timers.TryGetValue(id, out timer);
        }

        /// <summary>
        /// Destroys the current timer
        /// </summary>
        /// <param name="id"></param>
        public void DestroyTimer(uint id)
        {
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.Destroy();
                _timers.Remove(id);

                if (networkObject.IsServer)
                {
                    networkObject.SendRpc(RPC_DESTROY_CLIENT_TIMER_HANDLER, Receivers.Others, id);
                }
            }
        }


        /// <summary>
        /// Creates a timer object
        /// </summary>
        /// <returns>Timer Object</returns>
        private MatchNetworkTimer CreateNewTimer()
        {
            return CreateNewTimer(_timerCounter++);
        }
        
        /// <summary>
        /// Creates a timer with a specific ID. Used for client side creation and regular creation
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Timer Object</returns>
        private MatchNetworkTimer CreateNewTimer(uint id)
        {
            var timer = new MatchNetworkTimer(_timerCounter++, this.networkObject.Networker);

            if (networkObject.IsServer)
            {
                timer.NetworkStartEvent += NetworkStartTimer;
                timer.NetworkPauseEvent += NetworkPauseTimer;
                timer.NetworkResumeEvent += NetworkResumeTimer;
                timer.NetworkStopEvent += NetworkStopTimer;
            }
            
            return timer;
        }

        
        /// <summary>
        /// Sends all clients the network start event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkStartTimer(uint id, long stopTime)
        {
            networkObject.SendRpc(RPC_START_TIMER_HANDLER, Receivers.Others, id, stopTime);
        }
        
        
        /// <summary>
        /// Sends all clients the network pause event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkPauseTimer(uint id)
        {
            networkObject.SendRpc(RPC_PAUSE_TIMER_HANDLER, Receivers.Others, id);
        }
        
        
        /// <summary>
        /// Sends all clients the network resume event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkResumeTimer(uint id, long stopTime)
        {
            networkObject.SendRpc(RPC_RESUME_TIMER_HANDLER, Receivers.Others, id, stopTime);
        }
        
        
        /// <summary>
        /// Sends all clients the network stop event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkStopTimer(uint id)
        {
            networkObject.SendRpc(RPC_STOP_TIMER_HANDLER, Receivers.Others, id);
        }

        /// <summary>
        /// RPC Handler To start the timer
        /// </summary>
        /// <param name="args"></param>
        public override void StartTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }

            var id = args.GetAt<uint>(0);
            var stopTime = args.GetAt<long>(1);

            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.StartTimer(stopTime);
            }
        }

        /// <summary>
        /// RPC Handler to pause the timer
        /// </summary>
        /// <param name="args"></param>
        public override void PauseTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }
            
            var id = args.GetAt<uint>(0);
            
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.PauseTimer();
            }
        }

        /// <summary>
        /// RPC Handler to resume the timer
        /// </summary>
        /// <param name="args"></param>
        public override void ResumeTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }
            
            var id = args.GetAt<uint>(0);
            var stopTime = args.GetAt<long>(1);
            
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.ResumeTimer(stopTime);
            }
        }

        /// <summary>
        /// RPC Handler to stop the timer
        /// </summary>
        /// <param name="args"></param>
        public override void StopTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }
            
            var id = args.GetAt<uint>(0);
            
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.StopTimer();
            }
            
        }

        /// <summary>
        /// RPC Handler to create the timer
        /// </summary>
        /// <param name="args"></param>
        public override void CreateClientTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }

            var id = args.GetAt<uint>(0);

            CreateNewTimer(id);

        }

        /// <summary>
        /// RPC Handler to destroy the timer
        /// </summary>
        /// <param name="args"></param>
        public override void DestroyClientTimerRPCHandler(RpcArgs args)
        {
            if (networkObject.IsServer || !args.Info.SendingPlayer.IsHost)
            {
                return;
            }
            
            var id = args.GetAt<uint>(0);

            DestroyTimer(id);
        }
    }
}