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
        private float acceleration;
        private float maxSpeed;
        private float explosionRadius;
        private float explosionForce;
        private int team;

        private bool exploded;

        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private MatchCollisionFilter mcf;

        public void SetAttributes(float maxMissileTime,
            float acceleration,
            float maxSpeed,
            float explosionRadius,
            float explosionForce,
            Vector2 launchVelocity,
            MatchCollisionFilter mcf)
        {
            this.maxMissileTime = maxMissileTime;
            this.acceleration = acceleration;
            this.maxSpeed = maxSpeed;
            this.explosionRadius = explosionRadius;
            this.explosionForce = explosionForce;
            this.mcf.team = mcf.team;

            if (InstanceFinder.IsServer)
            {
                rb.AddForce(launchVelocity * 0.5f, ForceMode2D.Impulse);
                StartCoroutine(Move());
            }
        }

        public void OnSpawnEvent()
        {
            exploded = false;
        }

        private IEnumerator Move()
        {
            //var nearbyCols = new Collider2D[30];
            var startTime = Time.time;
            while (startTime + maxMissileTime > Time.time)
            {
                // Turn to nearest target
                // Currently disabled to re-evaluate if this is really wanted
                /*var numInRange = Physics2D.OverlapCircleNonAlloc(transform.position, homingRange, nearbyCols);
                Collider2D closestValid = null;
                float minDist = 0f;
                for (var i = 0; i < numInRange; i++)
                {
                    if (Vector2.Angle(transform.up,
                            nearbyCols[i]
                                .transform.position -
                            transform.position) >
                        homingAngle)
                    {
                        continue;
                    }

                    if (nearbyCols[i].TryGetComponent(out MatchCollisionFilter colMcf) && mcf.CanInteract(colMcf))
                    {
                        if (closestValid is null)
                        {
                            closestValid = nearbyCols[i];
                            minDist = Vector2.Distance(nearbyCols[i].transform.position, transform.position);
                        }
                        else
                        {
                            var thisDist = Vector2.Distance(nearbyCols[i].transform.position, transform.position);
                            if (thisDist < minDist)
                            {
                                closestValid = nearbyCols[i];
                                minDist = thisDist;
                            }
                        }
                    }
                }
                
                if (closestValid is not null)
                {
                    Debug.Log(closestValid.gameObject.name);
                    rb.AddTorque((Vector2.Angle(transform.up, closestValid.transform.position - transform.position) /
                                 homingAngle) * maxTorque * Time.deltaTime);
                }*/


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
            rb.velocity = Vector2.zero;
            rb.inertia = 0f;
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