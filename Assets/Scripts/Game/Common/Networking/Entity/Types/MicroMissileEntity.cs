using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using Game.Common.Instances;
using UnityEngine;
using UnityTemplateProjects.Game.Common.Gameplay;

namespace Game.Common.Networking.Entity.Types
{
    public class MicroMissileEntity: ProjectileEntity
    {
        private float maxMissileTime;
        private float homingAngle;
        private float acceleration;
        private float maxSpeed;
        private float explosionRadius;
        private float explosionForce;
        private int team;

        private bool exploded;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private MatchCollisionFilter mcf;

        public void SetAttributes(float maxMissileTime,
            float homingAngle,
            float acceleration,
            float maxSpeed,
            float explosionRadius,
            float explosionForce,
            MatchCollisionFilter mcf)
        {
            this.maxMissileTime = maxMissileTime;
            this.homingAngle = homingAngle;
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;
            this.explosionRadius = explosionRadius;
            this.explosionForce = explosionForce;
            this.mcf.team = mcf.team;
            
            if (InstanceFinder.IsServer)
            {
                StartCoroutine(Move());
            }
        }

        public void OnSpawnEvent()
        {
            exploded = false;
        }

        private IEnumerator Move()
        {
            var startTime = Time.time;
            while (startTime + maxMissileTime > Time.time)
            {
                rb.AddForce(transform.up * (acceleration * Time.deltaTime), ForceMode2D.Force);
                if (rb.velocity.magnitude > maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * maxSpeed;
                }
                
                yield return null;
            }
            Explode();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (InstanceFinder.IsServer)
            {
                if (other.gameObject.TryGetComponent(out MatchCollisionFilter collisionMcf)
                    && this.mcf.CanInteract(collisionMcf))
                {
                    Explode();
                }
            }
        }

        public void OnDespawnEvent()
        {
            //if(InstanceFinder.IsServer && !exploded) Explode();
        }

        public void Explode()
        {
            Physics2D.OverlapCircleAll(transform.position, explosionRadius)
                .Where(x => x.TryGetComponent(out MatchCollisionFilter otherMcf)
                    && this.mcf.CanInteract(otherMcf)).Select(x => x.attachedRigidbody).ToList()
                .ForEach(x =>
                {
                    x.AddForce((x.position - rb.position) * explosionForce, ForceMode2D.Impulse);
                });
            exploded = true;
            MainPersistantInstances.Get<EntityNetworkManager>().DespawnEntity(this);
        }
    }
}