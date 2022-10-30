using System.Collections.Generic;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        partial void ServerStart()
        {
            var receivers = new List<KeyValuePair<CommandType, ICommand>>();
            
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_DODGE, new ServerShipDodge()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_PRIMARY_FIRE_START, new ServerAgilityBeginPrimaryFire()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_PRIMARY_FIRE_END, new ServerAgilityEndPrimaryFire()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_PRIMARY_FIRE_START, new ServerHeavyBeginPrimaryFire()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_PRIMARY_FIRE_END, new ServerHeavyEndPrimaryFire()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_LOCKDOWN, new ServerHeavyLockdown()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_BURST, new ServerHeavyBurst()));

            _commandHandlerNetworkManager.InitializeServerCommands(receivers);
        }

        
        /*
        /// <summary>
        /// When the server receives client movement updates, it will assign the values to the ships themselves.
        /// </summary>
        /// <param name="args"></param>
        [Obsolete("Not used anymore, was just a test. All movement should be through fields")]
        partial void ServerMovementUpdate(RpcArgs args)
        {
            var movementVec = args.GetAt<Vector2>(0);
            var rotationVec = args.GetAt<float>(1);
            var playerID = args.Info.SendingPlayer.NetworkId;
            MainThreadManager.Run(() =>
            {
                var ship = masterSettings.GetShip(playerID);

                if (ship == null)
                {
                    Debug.LogError("Tried to get a ship from a player that does not have a ship");
                    return;
                }
                
                ship.shipController.movementVector = movementVec;
                ship.shipController.targetAngle = rotationVec;
            });
        }
        */
    }
}