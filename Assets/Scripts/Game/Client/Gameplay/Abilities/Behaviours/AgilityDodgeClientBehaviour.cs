using Game.Common.Gameplay.Abilities.Agility;
using UnityEngine;

namespace UnityTemplateProjects.Game.Client.Gameplay.Abilities.Behaviours
{
    public class AgilityDodgeClientBehaviour : RenderableAbilityBehaviour<AgilityDodgeAbility>
    {
        public override void Execute()
        {
            shipManager.simulationObject.representative.GetComponent<Animator>().SetTrigger("Dodge");
        }
    }
}