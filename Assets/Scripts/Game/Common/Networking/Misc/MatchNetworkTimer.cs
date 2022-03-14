using System;
using System.Diagnostics;
using BeardedManStudios.Forge.Networking;
using Debug = UnityEngine.Debug;

namespace Game.Common.Networking.Misc
{
    /// <summary>
    /// Class used to handle having a timer work across the network.
    /// </summary>
    public class MatchNetworkTimer
    {

        /// <summary>
        /// Called when the timer starts
        /// </summary>
        public delegate void StartEventHandler();
        public event StartEventHandler StartEvent;
        
        /// <summary>
        /// Called when the timer pauses
        /// </summary>
        public delegate void PauseEventHandler();
        public event PauseEventHandler PauseEvent;

        /// <summary>
        /// Called when the timer resumes
        /// </summary>
        public delegate void ResumeEventHandler();
        public event ResumeEventHandler ResumeEvent;

        /// <summary>
        /// Called when the timer stops
        /// </summary>
        public delegate void StopEventHandler();
        public event StopEventHandler StopEvent;
        
        /// <summary>
        /// Called when the timer is destroyed
        /// </summary>
        public delegate void DestroyEventHandler();
        public event DestroyEventHandler DestroyEvent;
        
        /// <summary>
        /// Used to start the timer over the network. Server to client.
        /// </summary>
        public delegate void NetworkStartEventHandler(uint id, long stopTime);
        public event NetworkStartEventHandler NetworkStartEvent;
        
        /// <summary>
        /// Used to pause the timer over the network. Server to client.
        /// </summary>
        public delegate void NetworkPauseEventHandler(uint id);
        public event NetworkPauseEventHandler NetworkPauseEvent;
        
        /// <summary>
        /// Used to resume the timer over the network. Server to client.
        /// </summary>
        public delegate void NetworkResumeEventHandler(uint id, long stopTime);
        public event NetworkResumeEventHandler NetworkResumeEvent;
        
        /// <summary>
        /// Used to stop the timer over the network. Server to client.
        /// </summary>
        public delegate void NetworkStopEventHandler(uint id);
        public event NetworkStopEventHandler NetworkStopEvent;

        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }
        
        public bool IsDestroyed { get; private set; }
        
        private readonly NetWorker _netWorker;
        private readonly Stopwatch _stopwatch;

        private long _stopTime;
        private long _timerLength;

        public readonly uint ID;
        
        
        
        public MatchNetworkTimer(uint id, NetWorker netWorker)
        {
            _stopwatch = new Stopwatch();
            _netWorker = netWorker;

            IsRunning = false;
            IsPaused = false;

            ID = id;
        }
        
        /// <summary>
        /// Call this to update the timer.
        /// </summary>
        /// <returns></returns>
        public void Tick()
        {
            
            if (IsDestroyed)
            {
                Debug.LogError("Timer is destroyed, can not be using it");
                return;
            }
            
            if (!_stopwatch.IsRunning) return;
            
            if (_netWorker.IsServer)
            {
                if (_stopwatch.ElapsedMilliseconds >= _timerLength)
                {
                    StopTimer();
                }
            }
            else
            {
                Debug.LogError("Ticking on the client is not allowed");
            }
        }


        /// <summary>
        /// Starts the timer for the network. Only use this function on the server.
        /// </summary>
        /// <param name="msLength">Length of time to go for. For client, it is when this timer should end</param>
        public void StartTimer(long msLength)
        {
            
            if (IsDestroyed)
            {
                Debug.LogError("Timer is destroyed, can not be using it");
                return;
            }
            
            if (msLength < 0)
            {
                Debug.LogError("Invalid time length, should be positive. Timer is not running");
                return;
            }

            if (IsPaused)
            {
                // TODO: Decide if this is intended behaviour or not
                Debug.LogWarning("Tried to start the timer while paused. Not intended behaviour, but will resume anyway.");
                
                ResumeTimer();
                return;
            }

            IsRunning = true;
            IsPaused = false;
            
            if (_netWorker.IsServer)
            {
                _timerLength = msLength;
                _stopTime = msLength;
                _stopwatch.Start();
                NetworkStartEvent?.Invoke(ID, _stopTime);
            }
            else
            {
                _timerLength = msLength;
                _stopTime = msLength;
                
                if (_timerLength < 0)
                {
                    StartEvent?.Invoke();
                    StopEvent?.Invoke();
                    return;
                }
                
                _stopwatch.Start();
            
                StartEvent?.Invoke();
            }
            
        }
        
        /// <summary>
        /// Pauses the timer.
        /// </summary>
        public void PauseTimer()
        {
            if (IsDestroyed)
            {
                Debug.LogError("Timer is destroyed, can not be using it");
                return;
            }
            
            IsRunning = false;
            IsPaused = true;
            
            _stopwatch.Stop();
            
            if (_netWorker.IsServer)
            {
                _timerLength -= _stopwatch.ElapsedMilliseconds;
                NetworkPauseEvent?.Invoke(ID);
            }
            
            PauseEvent?.Invoke();
        }

        /// <summary>
        /// Resumes the timer
        /// </summary>
        /// <param name="stopTime">Set when received network command from server</param>
        public void ResumeTimer(long stopTime = 0)
        {
            if (IsDestroyed)
            {
                Debug.LogError("Timer is destroyed, can not be using it");
                return;
            }
            
            if (!IsPaused)
            {
                Debug.LogWarning("Resumed timer that was not paused");
                return;
            }
            
            IsRunning = true;
            IsPaused = false;

            if (_netWorker.IsServer)
            {
                _stopTime = _timerLength;
                _stopwatch.Start();
                NetworkResumeEvent?.Invoke(ID, _stopTime);
            }
            else
            {
                _timerLength = stopTime;

                if (_timerLength < 0)
                {
                    StopTimer();
                }
                
                _stopwatch.Start();
            }
            
            ResumeEvent?.Invoke();
        }
        
        /// <summary>
        /// Stops the timer
        /// </summary>
        public void StopTimer()
        {

            if (IsDestroyed)
            {
                Debug.LogError("Timer is destroyed, can not be using it");
                return;
            }
            
            if (!IsRunning)
            {
                Debug.LogWarning("Stopped timer that was already stopped");
                return;
            }

            _timerLength = 0;
            IsRunning = false;
            IsPaused = false;
            
            _stopwatch.Stop();
            
            if (_netWorker.IsServer)
            {
                NetworkStopEvent?.Invoke(ID);
            }
            
            StopEvent?.Invoke();
            
        }

        /// <summary>
        /// Sets a flag that renders this timer unusable with exceptions.
        /// </summary>
        public void Destroy()
        {
            IsDestroyed = true;
            DestroyEvent?.Invoke();

            StartEvent = null;
            PauseEvent = null;
            ResumeEvent = null;
            StopEvent = null;
            DestroyEvent = null;

            NetworkStartEvent = null;
            NetworkPauseEvent = null;
            NetworkResumeEvent = null;
            NetworkStopEvent = null;
        }

        public long GetRemainingTime()
        {
            return _timerLength - _stopwatch.ElapsedMilliseconds;
        }
        
        /// <summary>
        /// Converts current timer time to time;
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return TimeSpan.FromMilliseconds(GetRemainingTime()).ToString();
        }
    }
}
