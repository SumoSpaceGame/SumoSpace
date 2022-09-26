using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BeardedManStudios.Forge.Networking;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Commands.Networkers;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Registry;
using Game.Common.Settings;
using UnityEngine;

namespace Game.Common.Networking.Commands
{
    public class CommandHandlerNetworkManager
    {
        private CommandHandler _commandHandler = new CommandHandler();

        private ICommandNetworker _commandNetworker;
        
        private bool _isServer;
        private ushort _clientMatchID;
        
        private MasterSettings _masterSettings;
        
        public CommandHandlerNetworkManager(NetworkObject networkObject, byte RPC_COMMAND, MasterSettings masterSettings)
        {
            _isServer = networkObject.IsServer;
            _masterSettings = masterSettings;
            _clientMatchID = _masterSettings.matchSettings.ClientMatchID;
            
            //Debug.Log(_networkerID);
            
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

            ushort shipID = rpcArgs.GetAt<ushort>(2);
            
            
            if (_isServer)
            {
                if (rpcArgs.Info.SendingPlayer.IsHost)
                {
                    Debug.LogError("Error tried to execute command that was sent from self");
                    return;
                }

                PlayerID playerID;

                if (!_masterSettings.playerIDRegistry.TryGetByNetworkID(rpcArgs.Info.SendingPlayer.NetworkId,
                        out playerID))
                {
                    Debug.LogError("Failed to handle command, player not registered");
                    return;
                }
                
                _commandHandler.ReceiveServer(commandType, GetPlayerShip(playerID.MatchID),
                    _commandNetworker, commandPacketData);
            }
            else
            {
                if (!rpcArgs.Info.SendingPlayer.IsHost)
                {
                    Debug.LogError("Error tried to execute command that was sent from self");
                    return;
                }
                
                _commandHandler.ReceiveClient(commandType, GetPlayerShip(shipID),
                    _commandNetworker, commandPacketData);
            }
        }


        private ShipManager GetPlayerShip(ushort matchID)
        {
            
            if (!_masterSettings.playerIDRegistry.TryGetByMatchID(matchID, out var playerID))
            {
                Debug.LogWarning("Could not receive command, invalid player ID");
                return null;
            }
            
            MainPersistantInstances.Get<AgentNetworkManager>()._playerShips.TryGet(playerID, out var data);
            return data;
            //return _masterSettings.GetShip(networkerID);
        }
        
        private ShipManager GetCurrentShip()
        {
            if (_isServer)
            {
                Debug.LogError("Can not get currentShip from server");
                return null;
            }
            
            //Debug.Log(_networkerID);
            var playerID = _masterSettings.playerIDRegistry.GetByMatchID(_clientMatchID);
            MainPersistantInstances.Get<AgentNetworkManager>()._playerShips.TryGet(playerID, out var data);
            return data;
            //return _masterSettings.GetShip(_networkerID);
        }
        
        
    }
}