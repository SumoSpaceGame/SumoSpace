using Game.Common.Gameplay.Ship;

namespace Game.Common.Gameplay.Commands
{
    /// <summary>
    /// Used to start the chain of commands
    /// Perform -> Receive Server -> Receive Client
    /// </summary>
    public interface ICommandPerformer : ICommand
    {
        /// <summary>
        /// Performs a command
        /// </summary>
        /// <param name="shipManager"></param>
        /// <param name="networker"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments);
    }
    
    
}
