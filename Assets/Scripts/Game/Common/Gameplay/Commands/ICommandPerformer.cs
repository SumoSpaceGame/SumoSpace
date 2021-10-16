using UnityEngine;
using Game.Common.Gameplay.Ship;

namespace Game.Common.Gameplay.Commands
{
    public interface ICommandPerformer : ICommand
    {
        bool Perform(ShipManager shipManager, ICommandNetworker networker);
    }
}
