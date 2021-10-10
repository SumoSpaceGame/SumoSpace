using BeardedManStudios.Forge.Networking;
using UnityEngine;

namespace Game.Common.Phases
{
    public interface IGamePhase
    {
        /// <summary>
        /// Called when phases are switched to. So if the next phase is switch to Phase2, Phase2.PhaseStart() will be called.
        /// </summary>
        void PhaseStart();
        
        /// <summary>
        /// Every framed this phase gets updated. This will be used to update what ever this phase requires. If this wants to change a phase, it will pass true to PhaseWantsNext()
        /// </summary>
        void PhaseUpdate();

        /// <summary>
        /// Cleans up this phase's dependencies for new usage.
        /// </summary>
        void PhaseCleanUp();
        
        /// <summary>
        /// Whenever a network update gets received. The phase will be notified.
        /// </summary>
        /// <param name="data"></param>
        void OnUpdateReceived(RPCInfo info, byte[] data);
    }
}
