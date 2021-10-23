using System.Collections.Generic;
using System.Runtime.InteropServices;
using BeardedManStudios.Forge.Networking;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Commands.Networkers;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking.Commands
{
    public class CommandHandlerNetworkManager
    {
        private CommandHandler _commandHandler = new CommandHandler();

        private ICommandNetworker _commandNetworker;
        
        private bool _isServer;
        private uint _networkerID;
        
        private MasterSettings _masterSettings;
        
        public CommandHandlerNetworkManager(NetworkObject networkObject, byte RPC_COMMAND, MasterSettings masterSettings)
        {
            _isServer = networkObject.IsServer;
            _masterSettings = masterSettings;
            _networkerID = networkObject.MyPlayerId;

            if (_isServer)
            {
                _commandNetworker = new ServerCommandNetworker(networkObject, RPC_COMMAND);
            }
            else
            {
                _commandNetworker = new ClientCommandNetworker(networkObject, RPC_COMMAND);
            }
        }

        public void InitializeClientCommands(IList<KeyValuePair<CommandType, ICommandPerformer>> commands)
        {
            _commandHandler.InitializePerformers(commands);
        }
        
        public void InitializeServerCommands(IList<KeyValuePair<CommandType, ICommand>> commands)
        {
            _commandHandler.InitializeReceivers(commands);
        }

        public bool Perform(CommandType commandType, params object[] arguments)
        {
            var ship = GetCurrentShip();

            return _commandHandler.Perform(commandType, ship, _commandNetworker/*, arguments*/);
        }

        public void HandleRPC(RpcArgs rpcArgs)
        {
            
            CommandType commandType = (CommandType) rpcArgs.GetAt<int>(0);

            CommandPacketData commandPacketData = CommandPacketData.Create(rpcArgs.GetAt<byte[]>(1));
            
            if (_isServer)
            {
                _commandHandler.ReceiveServer(commandType, GetPlayerShip(rpcArgs.Info.SendingPlayer.NetworkId),
                    _commandNetworker, commandPacketData);
            }
            else
            {
                _commandHandler.ReceiveClient(commandType, GetPlayerShip(rpcArgs.Info.SendingPlayer.NetworkId),
                    _commandNetworker, commandPacketData);
            }
        }


        private ShipManager GetPlayerShip(uint networkerID)
        {
            return _masterSettings.GetShip(networkerID);
        }
        
        private ShipManager GetCurrentShip()
        {
            if (_isServer)
            {
                Debug.LogError("Can not get currentShip from server");
                return null;
            }

            return _masterSettings.GetShip(_networkerID);
        }
        
        
    }
}