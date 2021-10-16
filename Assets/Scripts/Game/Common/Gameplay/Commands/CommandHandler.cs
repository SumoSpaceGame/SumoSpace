using System.Collections.Generic;
using Game.Common.Gameplay.Ship;

namespace Game.Common.Gameplay.Commands
{
    public class CommandHandler
    {

        private Dictionary<CommandType, ICommandPerformer> commandPerformers =
            new Dictionary<CommandType, ICommandPerformer>();

        private Dictionary<CommandType, ICommand> commandReceivers = new Dictionary<CommandType, ICommand>();


        public void InitalizePeformers(IList<KeyValuePair<CommandType, ICommandPerformer>> commands)
        {
            foreach (var commandPair in commands)
            {
                commandPerformers.Add(commandPair.Key, commandPair.Value);
            }
        }
        
        public void InitalizeReceivers(IList<KeyValuePair<CommandType, ICommand>> commands)
        {
            foreach (var commandPair in commands)
            {
                commandReceivers.Add(commandPair.Key, commandPair.Value);
            }
        }
        
        public bool Perform(CommandType commandType, ShipManager shipManager, ICommandNetworker networker)
        {
            if (commandPerformers.TryGetValue(commandType, out var commandPerformer))
            {
                return commandPerformer.Perform(shipManager, networker);
            }
            return false;
        }

        public bool ReceiveClient(CommandType commandType, ShipManager shipManager, ICommandNetworker networker, CommandPacketData data)
        {
            
            if (commandPerformers.TryGetValue(commandType, out var commandPerformer))
            {
                return commandPerformer.Receive(shipManager, networker, data);
            }
            return false;
        }
        
        public bool ReceiveServer(CommandType commandType, ShipManager shipManager, ICommandNetworker networker, CommandPacketData data)
        {
            
            if (commandReceivers.TryGetValue(commandType, out var command))
            {
                return command.Receive(shipManager, networker, data);
            }
            return false;
        }
        
    }
}
