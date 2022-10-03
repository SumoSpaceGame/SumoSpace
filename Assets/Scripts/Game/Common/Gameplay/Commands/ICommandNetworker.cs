﻿using Game.Common.Registry;

namespace Game.Common.Gameplay.Commands
{
    /// <summary>
    /// The communicator that is used for 
    /// </summary>
    public interface ICommandNetworker
    {
        /// <summary>
        /// CommandPacketData to be sent across the network
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        bool SendData(CommandPacketData data, CommandType commandID, PlayerID shipID);
    }
}