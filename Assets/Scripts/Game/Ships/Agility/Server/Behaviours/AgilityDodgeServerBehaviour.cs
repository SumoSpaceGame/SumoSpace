using System.Collections;
using Game.Ships.Agility.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Server.Behaviours
{
    public class AgilityDodgeServerBehaviour : AbilityBehaviour<AgilityDodgeAbility>
    {
        
        private bool onCooldown = false;

        public override void Execute()
        {
            if (!onCooldown)
            {
                StartCoroutine(Dodge());
                StartCoroutine(Cooldown());
            }
        }

        private IEnumerator Dodge()
        {
            var startTime = Time.time;
            var startRot = shipManager.Rotation;
            var startPos = shipManager.Position;

            var targetPos = (Quaternion.AngleAxis(startRot, Vector3.forward) * Vector3.up).normalized * Ability.Distance + startPos;

            while ((Time.time - startTime) / Ability.Time <= 1.0f)
            {
                shipManager.transform.position =
                    Vector3.Lerp(startPos, targetPos, (Time.time - startTime) / Ability.Time);
                yield return null;
            }
        }
        
        private IEnumerator Cooldown()
        {
            onCooldown = true;
            var startTime = Time.time;
            while (startTime + Ability.Cooldown > Time.time) yield return null;
            onCooldown = false;
        }
    }
}