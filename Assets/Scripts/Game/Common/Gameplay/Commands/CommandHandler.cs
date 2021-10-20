using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Gameplay.Commands
{
    public class CommandHandler
    {

        private Dictionary<CommandType, ICommandPerformer> commandPerformers =
            new Dictionary<CommandType, ICommandPerformer>();

        private Dictionary<CommandType, ICommand> commandReceivers = new Dictionary<CommandType, ICommand>();


        /// <summary>
        /// Initialize performer commands (commands that perform actions, aka client commands)
        /// </summary>
        /// <param name="commands"></param>
        public void InitializePerformers(IList<KeyValuePair<CommandType, ICommandPerformer>> commands)
        {
            foreach (var commandPair in commands)
            {
                commandPerformers.Add(commandPair.Key, commandPair.Value);
            }
        }
        
        /// <summary>
        /// Initialize receiver commands. You should only supply server side commands, ICommandPerformers are already made into
        /// </summary>
        /// <param name="commands"></param>
        public void InitializeReceivers(IList<KeyValuePair<CommandType, ICommand>> commands)
        {
            foreach (var commandPair in commands)
            {
                if (commandPair.Value.GetType().IsInstanceOfType(typeof(ICommandPerformer)))
                {
                    Debug.LogError("Tried to add performer to receivers");
                    return;
                }
                
                commandReceivers.Add(commandPair.Key, commandPair.Value);
            }
        }
        
        /// <summary>
        /// Perform the commandType. This should be executed only on the client.
        /// </summary>
        /// <param name="commandType">Command Type Enum</param>
        /// <param name="shipManager">Instance of Ship Manager for the ship that is executing the command</param>
        /// <param name="networker">Networker to transfer data between performers and receivers, and then from receivers to performer's receiver</param>
        /// <param name="arguments">Any extra data to pass along, E.X. bool to toggle</param>
        /// <returns>If successfully executed or not</returns>
        public bool Perform(CommandType commandType, ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            if (commandPerformers.TryGetValue(commandType, out var commandPerformer))
            {
                return commandPerformer.Perform(shipManager, networker, arguments);
            }
            Debug.LogError("Failed to grab command type from client performer- " + commandType);
            return false;
        }
        
        /// <summary>
        /// Receives the client's perform action.
        /// </summary>
        /// <param name="commandType">Command Type Enum</param>
        /// <param name="shipManager">Instance of Ship Manager for the ship that is executing the command</param>
        /// <param name="networker">Networker to transfer data between performers and receivers, and then from receivers to performer's receiver</param>
        /// <param name="data">Data packet, information that is being sent between the network</param>
        /// <returns>If successfully executed or not</returns>
        public bool ReceiveServer(CommandType commandType, ShipManager shipManager, ICommandNetworker networker, CommandPacketData data)
        {
            
            if (commandReceivers.TryGetValue(commandType, out var command))
            {
                return command.Receive(shipManager, networker, data);
            }
            Debug.LogError("Failed to grab command type from server - " + commandType);
            return false;
        }

        /// <summary>
        /// Receives the command infromation from the client. This is happens last. 
        /// </summary>
        /// <param name="commandType">Command Type Enum</param>
        /// <param name="shipManager">Instance of Ship Manager for the ship that is executing the command</param>
        /// <param name="networker">Networker to transfer data between performers and receivers, and then from receivers to performer's receiver</param>
        /// <param name="data">Data packet, information that is being sent between the network</param>
        /// <returns>If successfully executed or not</returns>
        public bool ReceiveClient(CommandType commandType, ShipManager shipManager, ICommandNetworker networker, CommandPacketData data)
        {
            
            if (commandPerformers.TryGetValue(commandType, out var commandPerformer))
            {
                return commandPerformer.Receive(shipManager, networker, data);
            }
            
            Debug.LogError("Failed to grab command type from client - " + commandType);
            return false;
        }

        
    }
}
