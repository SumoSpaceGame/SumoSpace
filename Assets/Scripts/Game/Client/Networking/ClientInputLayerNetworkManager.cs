
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
    {
        partial void ClientStart()
        {
            var performers = new List<KeyValuePair<CommandType, ICommandPerformer>>();
            
            //performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_DODGE, null));
            
            _commandHandlerNetworkManager.InitializeClientCommands(performers);
        }
        
        /// <summary>
        /// Used for the client to update the server on it's current movements.
        /// </summary>
        /// <param name="movementVec"></param>
        /// <param name="rotation"></param>
        public void SendMovementUpdate(Vector2 movementVec, float rotation)
        {
            if (networkObject.IsServer)
            {
                Debug.LogError("Send movement update activated on server. This shouldn't happen");
                return;
            }
            networkObject.SendRpcUnreliable(RPC_MOVEMENT_UPDATE, Receivers.Server, movementVec, rotation);
        }
        
        
    }
}
