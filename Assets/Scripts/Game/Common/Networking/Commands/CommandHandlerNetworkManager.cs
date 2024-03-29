﻿using System.Collections.Generic;
using FishNet.Connection;
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
        
        public CommandHandlerNetworkManager(InputLayerNetworkManager networkObject, MasterSettings masterSettings)
        {
            _isServer = networkObject.IsServer;
            _masterSettings = masterSettings;
            
            //Debug.Log(_networkerID);
            
            if (_isServer)
            {
                _commandNetworker = new ServerCommandNetworker(networkObject);
            }
            else
            {
                _commandNetworker = new ClientCommandNetworker(networkObject);
            }
        }

        public void InitializeClientCommands(IList<KeyValuePair<string, ICommandPerformer>> commands)
        {
            _commandHandler.InitializePerformers(commands);
        }
        
        public void InitializeServerCommands(IList<KeyValuePair<string, ICommand>> commands)
        {
            _commandHandler.InitializeReceivers(commands);
        }

        public bool Perform(string commandType, params object[] arguments)
        {
            var ship = GetCurrentShip();

            return _commandHandler.Perform(commandType, ship, _commandNetworker);
        }

        public void HandleRPC(string commandType, byte[] commandData, ushort shipID, NetworkConnection conn = null)
        {
            

            CommandPacketData commandPacketData = CommandPacketData.Create(commandData);

            int id = -1;

            if (conn != null) id = conn.ClientId;
            
            //Debug.Log($"Handling rpc from ${id} ${shipID} ${commandType}");
            
            if (_isServer)
            {
                PlayerID playerID;

                if (!_masterSettings.playerIDRegistry.TryGetByNetworkID(conn.ClientId,
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
            var playerID = _masterSettings.playerIDRegistry.GetByMatchID(_masterSettings.matchSettings.ClientMatchID);
            MainPersistantInstances.Get<AgentNetworkManager>()._playerShips.TryGet(playerID, out var data);
            return data;
            //return _masterSettings.GetShip(_networkerID);
        }
        
        
    }
}