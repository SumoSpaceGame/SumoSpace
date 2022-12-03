using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Registry;
using Game.Common.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class HeavyBurstServerBehaviour : AbilityBehaviour<HeavyBurstAbility>
{
    public void Start()
    {
        shipManager.OnHit += (force, _, _) => IncreaseCharge(force.magnitude);
    }


    // Apply force to every ship in the radius.
    public override void Execute()
    {
        ushort charge = shipManager.networkMovement.TempPassiveCharge;
        if (charge < Ability.MinCharge)
            return;
        Dictionary<PlayerID, ShipManager>.ValueCollection ships = MainPersistantInstances.Get<GameNetworkManager>().masterSettings.playerShips.GetAll();
        foreach (ShipManager ship in ships)
        {
            if (shipManager == ship)
                continue;
            Vector2 shipManagerToShip = ship.Position - shipManager.Position;
            float distance = shipManagerToShip.magnitude;
            float impulse = Ability.Impluse(charge, distance);
            if (impulse != 0)
                ship.OnHit(shipManagerToShip.normalized * impulse, ship.Position, ForceMode2D.Impulse);
        }
        shipManager.networkMovement.TempPassiveCharge = 0;
    }

    // Increases the charge based on the knockback received.
    private void IncreaseCharge(float force)
    {
        checked
        {
            try
            {
                shipManager.networkMovement.TempPassiveCharge += (ushort)(force * Ability.KnockbackToCharge);
            }
            catch (OverflowException)
            {
                shipManager.networkMovement.TempPassiveCharge = ushort.MaxValue;
            }
        }
    }
}