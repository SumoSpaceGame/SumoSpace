using System;
using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ShipDodge: ICommandPerformer {
    public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData) {
        if (manager.isPlayer) { // Client
            manager.simulationObject.representative.GetComponent<Animator>().SetTrigger("Dodge");
            Debug.Log("Dodge from me");
        } else { 
            manager.simulationObject.representative.GetComponent<Animator>().SetTrigger("Dodge");
            Debug.Log("Dodge not from me");
        }

        return true;
    }

    public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments) {
        Debug.Log("Dodge triggered");
        //networker.SendData(CommandPacketData.Create(new byte[]{}), (int)CommandType.AGILITY_DODGE, shipManager.playerMatchID);
        return true;
    }
}