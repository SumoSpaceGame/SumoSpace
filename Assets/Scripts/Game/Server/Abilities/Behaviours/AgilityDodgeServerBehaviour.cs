using System.Collections;
using Game.Common.Gameplay.Abilities.Agility;
using UnityEngine;

namespace UnityTemplateProjects.Game.Server.Abilities.Behaviours
{
    public class AgilityDodgeServerBehaviour : AbilityBehaviour<AgilityDodgeAbility>
    {

        public override void Execute()
        {
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
    }
}