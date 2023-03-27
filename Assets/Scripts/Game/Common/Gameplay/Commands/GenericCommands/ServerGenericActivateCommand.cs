using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Gameplay.Commands.GenericCommands
{
    public class ServerGenericActivateCommand : ICommand
    {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData) {
            // TODO: Create a way to get the packet data from a command and verify if a command should run
            shipManager.shipLoadout.ActivateCommand(shipManager, true, networker.commandType);
            
            networker.Send(packetData);
            return true;
        }

    }
}