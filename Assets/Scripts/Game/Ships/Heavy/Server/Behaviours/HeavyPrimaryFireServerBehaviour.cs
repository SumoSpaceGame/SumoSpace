using System;
using System.Collections;
using Game.Common.Gameplay.Ship;
using Game.Ships.Heavy.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Heavy.Server.Behaviours
{
    public class HeavyPrimaryFireServerBehaviour : AbilityBehaviour<HeavyPrimaryFireAbility> {
        private Coroutine coroutine;
    
        public override void Execute() {
            coroutine ??= shipManager.StartCoroutine(ServerSide());
            if (++oooCounter == 1) {
                executing = true;
            }
        }

        public override void Stop() {
            if (--oooCounter == 0) {
                executing = false;
            }
        }
    
        // TODO shoot from the correct points (not out front)
        private IEnumerator ServerSide() {
            var counter = 0f;
            var timer = 0f;
            while (counter >= 0) {
                counter += Time.deltaTime / Ability.Cooldown;
                timer = executing && !Ability.IsDisabled ? timer + Time.deltaTime : 0f;
                if (counter > 1 && executing && !Ability.IsDisabled) {
                    var t = shipManager.transform;
                    var hit = Physics2D.Raycast(t.position + t.up * 2, t.up);
                    if (hit.rigidbody) {
                        hit.rigidbody.AddForceAtPosition(t.up * Ability.CurrentKnockback(timer) * Time.deltaTime, hit.point, ForceMode2D.Force);
                        ShipManager sm = hit.rigidbody.GetComponent<ShipManager>();
                        HeavyBurstAbility targetBurstAbility = sm.shipLoadout.SecondaryAbility as HeavyBurstAbility;
                        if (targetBurstAbility != null)
                        {
                            checked
                            {
                                try
                                {
                                    sm.networkMovement.TempPassiveCharge += (ushort)(Ability.CurrentKnockback(timer) * targetBurstAbility.KnockbackToCharge);
                                }
                                catch (OverflowException)
                                {
                                    sm.networkMovement.TempPassiveCharge = ushort.MaxValue;
                                }
                            }
                        }
                    }
                    counter = 0;
                }
                yield return null;
            }
        }
    }
}