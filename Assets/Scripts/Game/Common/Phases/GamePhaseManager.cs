using UnityEngine;

namespace Game.Common.Phases
{
    /// <summary>
    /// Phases are a way to structure a single match in our game.
    /// Each action that involves interaction between the server and client is a phase.
    /// Make sure to take in account atomic network communications.
    /// So if the network sends data, the server and client HAVE to be ready to receive it.
    /// Otherwise a queue may be needed.
    ///
    /// This can be handled through ready states.
    /// Before switching, set mutex set ready to false, after switching set to true.
    /// If not ready, then go ahead and queue data until ready.
    /// </summary>
    public partial class GamePhaseManager 
    {
        public const int PHASE_MAX_ID = 11;
        private const int DEFAULT_PHASE_START = 0;
        // Max amount of switches per update
        private const int MAX_LOOPS_PER_UPDATE = 20;
    
        
        // Public Variables
        public Phase CurrentPhase { get; private set; } = Phase.MATCH_CONNECT;

        // Private Variables

        private int _currentSwitches = 0;
        
        // ENUM AND ENUM METHODS

        public bool WithinPhaseRange(int value)
        {
            return value >= 0 && value <= PHASE_MAX_ID;
        }
        
        // DELEGATES EVENTS
        
        
        /// <summary>
        /// Delegate event on Switch
        /// </summary>
        public delegate void PhaseSwitchEvent(Phase lastPhase, Phase nextPhase);
        public PhaseSwitchEvent OnPhaseSwitch;

        /// <summary>
        /// Delegate event on Update
        /// </summary>
        public delegate void PhaseUpdateEvent();
        public PhaseUpdateEvent OnPhaseUpdate;

        
        // PHASE MANAGEMENT


        /// <summary>
        /// Updates the phase, used to manage logic internally to the phase. Phases should switch themselves.
        /// </summary>
        public void UpdatePhase()
        {
            _currentSwitches = 0;
            OnPhaseUpdate?.Invoke();
        }

        /// <summary>
        /// Switches to a different phase
        /// </summary>
        /// <param name="phase"></param>
        public void SwitchPhase(Phase phase)
        {
            _currentSwitches++;

            if (_currentSwitches > MAX_LOOPS_PER_UPDATE)
            {
                Debug.LogError("Failed to switch phase, TOO MANY SWITCHES PER UPDATE.\n" +
                               $"Last phase - {CurrentPhase}, Next phase - {phase}");
                return;
            }
            Debug.Log("Switching to phase - " + phase.ToString());
            var lastPhase = CurrentPhase;
            CurrentPhase = phase;
            OnPhaseSwitch?.Invoke(lastPhase, CurrentPhase);
        }

        /// <summary>
        /// Pushes the phase forward to the next
        /// </summary>
        public void NextPhase()
        {
            
            var lastPhase = CurrentPhase;

            if (lastPhase < 0)
            {
                Debug.LogError("Phase ID can not be lower than 0");
                return;
            }
            
            if ((int) CurrentPhase >= PHASE_MAX_ID)
            {
                CurrentPhase = DEFAULT_PHASE_START;
            }
            else
            {
                CurrentPhase = CurrentPhase + 1;
            }
            Debug.Log("Switching to phase - " + CurrentPhase);
            
            
            OnPhaseSwitch?.Invoke(lastPhase, CurrentPhase);
        }
        
        
        
    
    }

    
}
