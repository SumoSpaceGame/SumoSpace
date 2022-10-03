
using System.Collections.Generic;
using FishNet.Object;
using Game.Common.Gameplay.Commands;
using Game.Common.Instances;
using UnityTemplateProjects.Game.Client.Gameplay.Abilities.CommandPerformers;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : NetworkBehaviour, IGamePersistantInstance
    {
        partial void ClientStart()
        {
            var performers = new List<KeyValuePair<CommandType, ICommandPerformer>>();
            
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_DODGE, new ClientAgilityDodge()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_PRIMARY_FIRE_START, new ClientAgilityBeginPrimaryFire()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.AGILITY_PRIMARY_FIRE_END, new ClientAgilityEndPrimaryFire()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_PRIMARY_FIRE_START, new ClientHeavyBeginPrimaryFire()));
            performers.Add(new KeyValuePair<CommandType, ICommandPerformer>(CommandType.HEAVY_PRIMARY_FIRE_END, new ClientHeavyEndPrimaryFire()));
            
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
