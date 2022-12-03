using FishNet.Object;
using FishNet.Object.Synchronizing;

namespace Game.Common.Gameplay.Ship
{
    /// <summary>
    /// Common values used between all ships
    /// If you need a custom values, we'll add them here. Each ship uses their values differently 
    /// </summary>
    public class ShipValues : NetworkBehaviour
    {
        /// <summary>
        /// For passive tracking
        /// Used for: Heavy, Utility
        /// </summary>
        [SyncVar(SendRate = 0.0f)] public ushort passiveValue;
        
        
    }
}