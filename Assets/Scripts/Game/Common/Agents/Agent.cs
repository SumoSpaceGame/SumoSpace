using System;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Agents
{
    public abstract class Agent : AgentBehavior
    {
        // Core defining values
        public uint id = 0;
        public bool isClientOwned = false;


        protected override void NetworkStart()
        {
            base.NetworkStart();

            if (this.networkObject.IsServer && !this.networkObject.IsOwner)
            {
                networkObject.Destroy();
            }
        }


        /// <summary>
        /// Starts when initiated
        /// </summary>
        public abstract void AgentStart();
        /// <summary>
        /// Called through agent manager every update
        /// </summary>
        public abstract void AgentUpdate();
        /// <summary>
        /// To send RPCs on a fixed interval (mostly should be used for unreliable rpc's).
        /// </summary>
        public abstract void AgentFixedSendRPC();
        /// <summary>
        /// Called when agent manager wants this destroy. Make sure to clean up everything spawned.
        /// </summary>
        public abstract void AgentDestroy();

        public virtual void OnChangeOwnership()
        {
            
        }

    }
}
