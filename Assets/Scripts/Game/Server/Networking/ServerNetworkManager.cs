using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Agents;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Phases;
using UnityEngine;
using Phase = Game.Common.Phases.Phase;

// Partial class namespace requirement
// ReSharper disable once CheckNamespace
namespace Game.Common.Networking
{
    public partial class GameNetworkManager : GameManagerBehavior
    {
        


        partial void OnServerNetworkStart()
        {
                
            networkObject.Networker.playerAccepted += ServerOnPlayerConnected ;
        }
        
        
        
        private void ServerOnPlayerConnected (NetworkingPlayer player, NetWorker sender)
        {
            return;
            MainThreadManager.Run(() =>
            {
                AgentBehavior agent = NetworkManager.Instance.InstantiateAgent(0, new Vector3(Random.value * 5.0f,Random.value * 5.0f,Random.value * 5.0f));
                var agentManager = MainPersistantInstances.Get<AgentManager>();
                agentManager.SendOwnership(agent.networkObject.NetworkId, player);

            });
        }
        
    }
}
