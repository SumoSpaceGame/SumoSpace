using System;
using System.Collections;
using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Registry;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    public ShipPrefabList shipPrefabList;

    public LayerMask player, team, enemy;
    
    public ShipManager SpawnShip(PlayerID playerID, int shipType, bool isPlayer, bool isEnemy)
    {
        var prefab = shipPrefabList.GetShip(shipType);
        var shipClone = Instantiate(prefab);

        var shipClass = shipClone.GetComponent<ShipManager>();

        shipClass.isPlayer = isPlayer;
        shipClass.playerMatchID = playerID;

        if (isEnemy && isPlayer)
        {
            Debug.LogError("Ship spawned being enemy and player, should not be possible");
        }

        shipClass.SetLayer( isPlayer ? player : isEnemy ? enemy : team);
        
        return shipClass;
    }
}
