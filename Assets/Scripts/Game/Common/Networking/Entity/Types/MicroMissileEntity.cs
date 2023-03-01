using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using FishNet;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using UnityEngine;
using UnityTemplateProjects.Game.Common.Gameplay;

namespace Game.Common.Networking.Entity.Types
{
    public class MicroMissileEntity: ProjectileEntity
    {
        private float _maxMissileTime;
        private float _acceleration;
        private float _maxSpeed;
        private float _explosionRadius;
        private float _explosionForce;
        private int _team;

        private bool _exploded;

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
            _maxMissileTime = maxMissileTime;
            _acceleration = acceleration;
            _maxSpeed = maxSpeed;
            _explosionRadius = explosionRadius;
            _explosionForce = explosionForce;
            this.mcf.team = mcf.team;

            if (!InstanceFinder.IsServer) return;
            rb.AddForce(launchVelocity * 0.5f, ForceMode2D.Impulse);
            StartCoroutine(Move());
        }

        public void OnSpawnEvent()
        {
            _exploded = false;
        }

        private IEnumerator Move()
        {
            //var nearbyCols = new Collider2D[30];
            var startTime = Time.time;
            while (startTime + _maxMissileTime > Time.time)
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


                rb.AddForce(transform.up * (_acceleration * Time.deltaTime), ForceMode2D.Force);
                if (rb.velocity.magnitude > _maxSpeed)
                {
                    rb.velocity = rb.velocity.normalized * _maxSpeed;
                }
                
                yield return null;
            }
            Explode();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!InstanceFinder.IsServer) return;
            if (other.gameObject.TryGetComponent(out MatchCollisionFilter collisionMcf) && mcf.CanInteract(collisionMcf))
            {
                Explode();
            }
        }

        public void OnDespawnEvent()
        {
            //if(InstanceFinder.IsServer && !exploded) Explode();
            rb.velocity = Vector2.zero;
            rb.inertia = 0f;
        }

        private void Explode()
        {
            // ReSharper disable once Unity.PreferNonAllocApi
            Physics2D.OverlapCircleAll(transform.position, _explosionRadius)
                .Where(x => x.TryGetComponent(out MatchCollisionFilter otherMcf)
                    && this.mcf.CanInteract(otherMcf)).Select(x => x.attachedRigidbody).ToList()
                .ForEach(x =>
                {
                    if (x.TryGetComponent<ShipManager>(out var shipManager))
                        shipManager.OnHit((x.position - rb.position).normalized * _explosionForce, rb.position, ForceMode2D.Impulse);
                    else
                        x.AddForce((x.position - rb.position) * _explosionForce, ForceMode2D.Impulse);
                });
            _exploded = true;
            MainPersistantInstances.Get<EntityNetworkManager>().DespawnEntity(this);
        }
    }
}