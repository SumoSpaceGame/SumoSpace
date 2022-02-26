using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;

public class ServerShipDodge: ICommand {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        manager.shipLoadout.PrimaryAbility.Execute(manager, true);
        networker.SendData(packetData, (int)CommandType.AGILITY_DODGE, manager.playerMatchID);
        return true;
    }
}