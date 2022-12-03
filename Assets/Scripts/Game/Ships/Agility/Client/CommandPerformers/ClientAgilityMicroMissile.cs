using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Ships.Agility.Client.CommandPerformers
{
    public class ClientAgilityMicroMissile : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, ICommandNetworker networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            networker.SendData(CommandPacketData.Create(new byte[] { }), CommandType.AGILITY_MICRO_MISSLES,
                shipManager.playerMatchID);
            return true;
        }
    }
}
