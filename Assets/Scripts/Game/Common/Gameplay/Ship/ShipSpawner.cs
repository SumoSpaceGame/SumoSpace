using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public ShipPrefabList shipPrefabList;
    

    public Ship SpawnShip(ushort playerMatchID, int shipType, bool isPlayer)
    {
        var prefab = shipPrefabList.GetShip(shipType);
        var shipClone = Instantiate(prefab);

        var shipClass = shipClone.GetComponent<Ship>();

        shipClass.isPlayer = isPlayer;
        shipClass.playerMatchID = playerMatchID;


        return shipClass;
    }
}
