using System.Collections.Generic;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using Game.Ships.Agility.Server.CommandPerformers;
using Game.Ships.Heavy.Server.CommandPerformers;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        partial void ServerStart()
        {
            var receivers = new List<KeyValuePair<CommandType, ICommand>>();
            
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_DODGE, new AgilityShipDodgeServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_PRIMARY_FIRE_START, new AgilityBeginPrimaryFireServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_PRIMARY_FIRE_END, new AgilityEndPrimaryFireServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.AGILITY_MICRO_MISSLES, new AgilityMicroMissileServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_PRIMARY_FIRE_START, new HeavyBeginPrimaryFireServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_PRIMARY_FIRE_END, new HeavyEndPrimaryFireServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_LOCKDOWN, new HeavyLockdownServerCommand()));
            receivers.Add(new KeyValuePair<CommandType, ICommand>(CommandType.HEAVY_BURST, new HeavyBurstServerCommand()));

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