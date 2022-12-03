using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
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
                missile.SetAttributes(
                    Ability.MaxMissileTime,
                    Ability.HomingAngle,
                    Ability.Acceleration,
                    Ability.MaxSpeed,
                    Ability.ExplosionRadius,
                    Ability.ExplosionForce,
                    shipManager.matchCollisionFilter);
                missile.transform.position = shipManager.Position;
                missile.transform.up = Quaternion.AngleAxis(shipManager.Rotation, Vector3.forward) * Vector3.up;
                
                //missile.GetComponent<>();
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
