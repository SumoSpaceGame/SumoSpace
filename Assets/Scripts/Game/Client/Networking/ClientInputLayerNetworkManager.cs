
using System.Collections.Generic;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using Game.Ships.Agility.Client.CommandPerformers;
using Game.Ships.Heavy.Client.CommandPerformers;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        partial void ClientStart()
        {
            var performers = new List<KeyValuePair<CommandType, ICommandPerformer>>();
            
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_DODGE, new AgilityDodgeClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_PRIMARY_FIRE_START, new AgilityBeginPrimaryFireClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_PRIMARY_FIRE_END, new AgilityEndPrimaryFireClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_MICRO_MISSLES, new AgilityMicroMissileClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_PRIMARY_FIRE_START, new HeavyBeginPrimaryFireClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_PRIMARY_FIRE_END, new HeavyEndPrimaryFireClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_LOCKDOWN, new HeavyLockdownClientCommand()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_BURST, new HeavyBurstClientCommand()));

            _commandHandlerNetworkManager.InitializeClientCommands(performers);
        }

        public void PerformCommand(CommandType type, byte[] data) {
            _commandHandlerNetworkManager.Perform(type, data);
        }

        public void SendCommand(CommandType type, byte[] extra) {
            PerformCommand(type, extra);
        }
    }
}
