using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Common.Gameplay.Commands
{
    public interface ICommand
    {
        bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData);
    }
}
