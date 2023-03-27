using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

namespace Game.Ships.Agility.Client.CommandPerformers
{
    public class ClientAgilityMicroMissile : ICommandPerformer
    {
        public bool Receive(ShipManager shipManager, CommandNetworkerData networker, CommandPacketData packetData)
        {
            shipManager.shipLoadout.SecondaryAbility.Execute(shipManager, false);
            return true;
        }

        public bool Perform(ShipManager shipManager, CommandNetworkerData networker, params object[] arguments)
        {
            networker.Send();
            return true;
        }
    }
}
