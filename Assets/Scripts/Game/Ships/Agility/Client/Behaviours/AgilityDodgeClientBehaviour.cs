using Game.Ships.Agility.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Client.Behaviours
{
    public class AgilityDodgeClientBehaviour : RenderableAbilityBehaviour<AgilityDodgeAbility>
    {
        public override void Execute()
        {
            shipManager.simulationObject.representative.GetComponent<Animator>().SetTrigger("Dodge");
        }
    }
}