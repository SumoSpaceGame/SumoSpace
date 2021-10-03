using System;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Agents;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.BasicCube
{
    [RequireComponent(typeof(Rigidbody))]
    public class BasicCubeController : Agent
    {

        private Rigidbody _rigidbody;
        public float AgentSpeed = 2.5f;

        
        

        protected override void NetworkStart()
        {
            base.NetworkStart();
            
            networkObject.onDestroy += onNetworkDestroy;
            networkObject.UpdateInterval = 16; // 60hz is 16.6, so this is going 62.5hz
            this.networkObject.rotation = Quaternion.Euler(0, 0, 0);
            
            MainThreadManager.Run(() =>
            {
                MainPersistantInstances.Get<AgentManager>().AddAgent(this.networkObject.NetworkId, this);
            });
        }


        private void OnVelocityChange(Vector3 vel)
        {
            _rigidbody.velocity = vel;
        }
        
        public override void AgentStart()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void AgentUpdate()
        {
            if (isClientOwned)
            {
                Vector3 Displacement = Vector3.zero;

                if (Input.GetKey(KeyCode.A))
                {
                    Displacement += new Vector3(1.0f, 0.0f, 0.0f);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Displacement -= new Vector3(1.0f, 0.0f, 0.0f);
                }
                
                if (Input.GetKey(KeyCode.W))
                {
                    Displacement += new Vector3(0.0f, 0.0f, 1.0f);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Displacement -= new Vector3(0.0f, 0.0f, 1.0f);
                }
                
                Debug.Log(Displacement);
               Displacement.Normalize();
               Debug.Log(Displacement);
               Displacement *= AgentSpeed * Time.deltaTime;
               Debug.Log(Displacement);
               if(Displacement.magnitude > 0) _rigidbody.velocity = Displacement;


            }
            else
            {
                _rigidbody.position = this.networkObject.position;
                _rigidbody.rotation = this.networkObject.rotation;
                //_rigidbody.velocity = Vector3.zero;
                
            }
        }

        public override void AgentFixedSendRPC()
        {
            if (isClientOwned)
            {
                networkObject.SendRpc(RPC_UPDATE_MOVE, Receivers.Server, 
                    _rigidbody.position, _rigidbody.rotation, _rigidbody.velocity);
            }
        }
        public override void OnChangeOwnership()
        {
            base.OnChangeOwnership();
        }


        public override void AgentDestroy()
        {
            Destroy(this.gameObject);
        }


        private void onNetworkDestroy(NetWorker netWorker)
        {
            AgentDestroy();
            MainPersistantInstances.Get<AgentManager>().RemoveAgent(this.networkObject.NetworkId);
        }

        public override void UpdateMove(RpcArgs args)
        {
            var pos = args.GetAt<Vector3>(0);
            var rot = args.GetAt<Quaternion>(1);
            var vel = args.GetAt<Vector3>(2);
            
            MainThreadManager.Run(() =>
            {
                this._rigidbody.position = pos;
                this._rigidbody.rotation = rot;
                this._rigidbody.velocity = vel;

                this.networkObject.position = pos;
                this.networkObject.rotation = rot;
                this.networkObject.velocity = vel;
            });
        }
        
        
    }
}
