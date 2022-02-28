using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public ShipPrefabList shipPrefabList;
    

    public ShipManager SpawnShip(PlayerID playerID, int shipType, bool isPlayer)
    {
        var prefab = shipPrefabList.GetShip(shipType);
        var shipClone = Instantiate(prefab);

        var shipClass = shipClone.GetComponent<ShipManager>();

        shipClass.isPlayer = isPlayer;
        shipClass.playerMatchID = playerID;


        return shipClass;
    }
}
