using System.Collections.Generic;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Networking;
using Game.Common.Registry;
using Game.Ships.Heavy.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Heavy.Server.Behaviours
{
    public class HeavyBurstServerBehaviour : AbilityBehaviour<HeavyBurstAbility>
    {
        // Placeholder charge until the bar system is implemented.
        private float charge = 1;

        // Apply force to every ship in the radius.
        public override void Execute()
        {
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
                    ship._rigidbody2D.AddForce(shipManagerToShip.normalized * impulse, ForceMode2D.Impulse);
            }
            // TODO: Remove charge used.
        }
    }
}