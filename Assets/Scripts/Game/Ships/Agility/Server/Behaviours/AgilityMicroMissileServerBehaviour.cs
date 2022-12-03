using System.Collections;
using System.Collections.Generic;
using Game.Ships.Agility.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Server.Behaviours
{
    public class AgilityMicroMissileServerBehaviour : AbilityBehaviour<AgilityMicroMissileAbility>
    {

        public override void Execute()
        {
            shipManager.StartCoroutine(MicroMissile());
        }

        private IEnumerator MicroMissile()
        {
            // Shoot missiles
            yield return null;
        }
    }
}
