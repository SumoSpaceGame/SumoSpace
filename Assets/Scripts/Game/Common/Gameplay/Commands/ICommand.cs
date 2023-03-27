using Game.Common.Gameplay.Ship;

namespace Game.Common.Gameplay.Commands
{
    /// <summary>
    /// Server side command
    /// </summary>
    public interface ICommand
    {
        bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData);
    }
}
