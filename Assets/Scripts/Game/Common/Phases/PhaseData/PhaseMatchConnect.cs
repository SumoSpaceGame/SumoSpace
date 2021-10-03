using System;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Agents;
using Game.Common.Networking;

namespace Game.Common.Phases.PhaseData
{
    public class PhaseMatchConnect
    {
        
        public const short PLAYER_NETWORK_INITIALIZED_FIN = 1; // Player's basic network initialization are ready.
        public const short MATCH_CANCELLED = 2; // Server cancelled the match
        /// <summary>
        /// Systems that need to be loaded and added to main instance for this client to be ready for all match stuff.
        /// </summary>
        public static readonly Type[] GAME_NETWORK_INITIALIZE_TYPES =
        {
            typeof(GamePhaseNetworkManager),
            typeof(GameNetworkManager),
            typeof(AgentManager)
        };
    }
}