using System.Collections;
using Game.Common.Gameplay.Abilities.Agility;
using UnityEngine;

namespace UnityTemplateProjects.Game.Server.Abilities.Behaviours
{
    public class AgilityDodgeServerBehaviour : AbilityBehaviour<AgilityDodgeAbility>
    {

        private Coroutine cooldownCoroutine;
        
        public override void Execute()
        {
            if (cooldownCoroutine != null)
            {
                return;
            }

            cooldownCoroutine = shipManager.StartCoroutine(Cooldown());
            shipManager.StartCoroutine(Dodge());
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
            var timer = 0f;
            while (timer >= 0)
            {
                timer += Time.deltaTime / Ability.Cooldown;
                yield return null;
            }
        }
    }
}