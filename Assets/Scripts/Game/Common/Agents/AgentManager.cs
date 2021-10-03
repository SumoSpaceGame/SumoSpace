using System;
using System.Collections.Generic;
using System.Diagnostics;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using Unity.Mathematics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Game.Common.Agents
{
    public class AgentManager : AgentManagerBehavior, IGamePersistantInstance
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        protected override void NetworkStart()
        {
            base.NetworkStart();
            MainThreadManager.Run(()=>{ 
                MainPersistantInstances.TryAdd(this);
            });
        }

        private Dictionary<uint, Agent> _agentList = new Dictionary<uint, Agent>();


        private Stopwatch fixedTimer = new Stopwatch();
        
        
        private void Update()
        {

            bool fixedUpdate = false;


            if (!fixedTimer.IsRunning)
            {
                fixedTimer.Start();
            }
            
            // TODO: Globalize update interval
            if (fixedTimer.ElapsedMilliseconds > 16)
            {
                fixedTimer.Restart();
                fixedUpdate = true;
            }
            
            foreach (var agentKeyPair in _agentList )
            {
                var agent = agentKeyPair.Value;
                agent.AgentUpdate();
                if(fixedUpdate) 
                    agent.AgentFixedSendRPC();
            }
        }

        private void OnDestroy()
        {
            
            MainPersistantInstances.Remove<AgentManager>();
            Destroy(this.gameObject);
        }


        /// <summary>
        /// Instantiates the agent by providing a prefab. This will be updated to work with asset bundles, but for prototyping this is the current way it works.
        /// </summary>
        /// <param name="agentID"></param>
        /// <param name="AgentPrefab"></param>
        private void InstantiateAgent(uint agentID, GameObject AgentPrefab, bool isServer = false)
        {
            var newAgentObject = Instantiate(AgentPrefab);

            var agentScript = newAgentObject.GetComponent<Agent>();

            if (agentScript == null)
            {
                Debug.LogError("Tried to instantiate agent without agent script");
                return;
            }
            
            _agentList.Add(agentID, agentScript);
            
            agentScript.id = agentID;
            agentScript.isClientOwned = !isServer;
            
            agentScript.AgentStart();
        }


        public void AddAgent(uint agentID, Agent agentScript)
        {
            _agentList.Add(agentID, agentScript);
            agentScript.id = agentID;
            agentScript.isClientOwned = !(!agentScript.networkObject.IsOwner || networkObject.IsServer);
            agentScript.AgentStart();
        }
        
        public void RemoveAgent(uint agentID)
        {
            _agentList.Remove(agentID);
        }
        
        // TODO: Create server pipeline to make sure everything is sent synchronized. If not make sure this gets queued
        public override void GiveClientOwnership(RpcArgs args)
        {

            MainThreadManager.Run(() =>
            {
                var agentID = args.GetAt<uint>(0);
                Debug.Log("Receiving Ownership for " + agentID);
                if (!args.Info.SendingPlayer.IsHost) return;
                
                if (_agentList.TryGetValue(agentID, out var agent))
                {
                    agent.isClientOwned = true;
                
                }
            
            });
        }

        public void SendOwnership(uint agentID, NetworkingPlayer player)
        {
            Debug.Log("Sending ownership to " + agentID);
            networkObject.SendRpc(player ,RPC_GIVE_CLIENT_OWNERSHIP, agentID);
        }
    }
}
