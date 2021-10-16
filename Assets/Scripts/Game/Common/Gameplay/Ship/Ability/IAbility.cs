
using Game.Common.Gameplay.Ship;

public interface IAbility
{
    void Receive(ShipManager shipManager, string data);
}
