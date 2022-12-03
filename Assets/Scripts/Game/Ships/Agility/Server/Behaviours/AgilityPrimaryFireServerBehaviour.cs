using System;
using System.Collections;
using Game.Common.Gameplay.Ship;
using Game.Ships.Agility.Common.Abilities;
using Game.Ships.Heavy.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Server.Behaviours
{
    public class AgilityPrimaryFireServerBehaviour : AbilityBehaviour<AgilityPrimaryFireAbility>
    {
        private Coroutine coroutine;

        public override void Execute()
        {
            coroutine ??= shipManager.StartCoroutine(ServerSide());
            if (++oooCounter == 1)
            {
                executing = true;
            }
        }

        public override void Stop()
        {
            if (--oooCounter == 0)
            {
                executing = false;
            }
        }

        // TODO shoot from the correct points (not out front)
        private IEnumerator ServerSide()
        {
            var counter = 0f;
            while (counter >= 0)
            {
                counter += Time.deltaTime / Ability.Cooldown;
                if (counter > 1 && executing)
                {
                    var t = shipManager.transform;
                    var hit = Physics2D.Raycast(t.position + t.up * 2, t.up);
                    if (hit.rigidbody)
                    {
                        hit.rigidbody.GetComponent<ShipManager>().OnHit(t.up * Ability.Knockback, hit.point, ForceMode2D.Impulse);\
                    }
                    counter = 0;
                }
                yield return null;
            }
        }
    }
}
