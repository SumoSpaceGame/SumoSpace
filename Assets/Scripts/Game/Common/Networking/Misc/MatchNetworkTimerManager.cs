using System.Collections.Generic;
using System.Linq;
using FishNet.Object;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking.Misc
{
    public class MatchNetworkTimerManager : NetworkBehaviour, IGamePersistantInstance
    {
        private Dictionary<uint, MatchNetworkTimer> _timers = new Dictionary<uint, MatchNetworkTimer>();
        
        private uint _timerCounter = 0;

        

        public override void OnStartNetwork()
        {
            base.OnStartNetwork();
            MainPersistantInstances.Add(this);
        }

        private void OnDestroy()
        {
            MainPersistantInstances.Remove<MatchNetworkTimerManager>();
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
                
                if(NetworkObject.IsServer) timer.Tick();
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
            if (!NetworkObject.IsServer)
            {
                Debug.LogError("Can not create timer on the client, has to be on the server");
                return null;
            }
            
            var timer = CreateNewTimer();
            
            CreateClientTimerRPCHandler(timer.ID);
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
        /// Returns the current set of timers
        /// </summary>
        /// <returns></returns>
        public MatchNetworkTimer[] GetTimers()
        {
            return _timers.Values.ToArray();
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

                if (NetworkManager.IsServer)
                {
                    DestroyClientTimerRPCHandler(id);
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
            var timer = new MatchNetworkTimer(id, NetworkObject);

            if (NetworkObject.IsServer)
            {
                timer.NetworkStartEvent += NetworkStartTimer;
                timer.NetworkPauseEvent += NetworkPauseTimer;
                timer.NetworkResumeEvent += NetworkResumeTimer;
                timer.NetworkStopEvent += NetworkStopTimer;
            }
            
            _timers.Add(timer.ID, timer);
            
            return timer;
        }

        
        /// <summary>
        /// Sends all clients the network start event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkStartTimer(uint id, long stopTime)
        {
            StartClientTimerRPCHandler(id, stopTime);
        }
        
        
        /// <summary>
        /// Sends all clients the network pause event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkPauseTimer(uint id, long pauseTime)
        {
            PauseClientTimerRPCHandler(id, pauseTime);
        }
        
        
        /// <summary>
        /// Sends all clients the network resume event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkResumeTimer(uint id, long stopTime)
        {
            ResumeClientTimerRPCHandler(id, stopTime);
        }
        
        
        /// <summary>
        /// Sends all clients the network stop event
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopTime"></param>
        private void NetworkStopTimer(uint id)
        {
            StopClientTimerRPCHandler(id);
        }

        /// <summary>
        /// RPC Handler To start the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void StartClientTimerRPCHandler(uint id, long stopTime)
        {
            Debug.Log("Starting timer on client" + _timers.Count + " " + id);
            
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                Debug.Log("Starting the things");
                timer.StartTimer(stopTime);
            }
        }

        /// <summary>
        /// RPC Handler to pause the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void PauseClientTimerRPCHandler(uint id, long pauseTime)
        {
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.PauseTimer(pauseTime);
            }
        }

        /// <summary>
        /// RPC Handler to resume the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void ResumeClientTimerRPCHandler(uint id, long stopTime)
        {
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.ResumeTimer(stopTime);
            }
        }

        /// <summary>
        /// RPC Handler to stop the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void StopClientTimerRPCHandler(uint id)
        {
            if (_timers.TryGetValue(id, out MatchNetworkTimer timer))
            {
                timer.StopTimer();
            }
            
        }

        /// <summary>
        /// RPC Handler to create the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void CreateClientTimerRPCHandler(uint id)
        {
            Debug.Log("Creating match timer on client " + id);

            CreateNewTimer(id);

        }

        /// <summary>
        /// RPC Handler to destroy the timer
        /// </summary>
        /// <param name="args"></param>
        [ObserversRpc]
        public void DestroyClientTimerRPCHandler(uint id)
        {
            DestroyTimer(id);
        }
        
    }
}