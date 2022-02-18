using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Gameplay.Commands
{
    /// <summary>
    /// Server side command
    /// </summary>
    public interface ICommand
    {
        bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData);
    }
}
