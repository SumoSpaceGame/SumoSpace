using System.Collections;
using Game.Common.Instances;
using Game.Common.Networking.Entity;
using Game.Common.Networking.Entity.Types;
using Game.Ships.Agility.Common.Abilities;
using UnityEngine;

namespace Game.Ships.Agility.Server.Behaviours
{
    public class AgilityMicroMissileServerBehaviour : AbilityBehaviour<AgilityMicroMissileAbility>
    {
        public GameObject missilePrefab;
        private bool onCooldown = false;
        
        public override void Execute()
        {
            if (!onCooldown)
            {
                StartCoroutine(ShootMissiles());
                StartCoroutine(Cooldown());
            }
        }

        private IEnumerator ShootMissiles()
        {
            for (var i = 0; i < Ability.NumMissiles; i++)
            {
                var missile = MainPersistantInstances.Get<EntityNetworkManager>().SpawnEntity(missilePrefab.GetComponent<MicroMissileEntity>()) as MicroMissileEntity;
                if (missile is null)
                {
                    yield break;
                }
                missile.transform.position = shipManager.Position + (shipManager.transform.right * ((i % 2 == 0 ? -1 : 1) * 0.4f));
                missile.transform.up = Quaternion.AngleAxis(shipManager.Rotation, Vector3.forward) * Vector3.up;
                missile.SetAttributes(
                    Ability.MaxMissileTime,
                    Ability.Acceleration,
                    Ability.MaxSpeed,
                    Ability.ExplosionRadius,
                    Ability.ExplosionForce,
                    shipManager._rigidbody2D.velocity,
                    shipManager.matchCollisionFilter);
                
                yield return new WaitForSeconds(Ability.AttackDuration / Ability.NumMissiles);
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
