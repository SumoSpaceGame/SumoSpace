using System.Collections;
using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(ShipManager))]
    public partial class ShipController : MonoBehaviour
    {
        //private ShipMovement sm;
        private ShipLoadout loadout;
        private ShipMovement movement;

        [HideInInspector]
        public Vector2 movementVector;
        [HideInInspector]
        public float targetAngle;
        
        private Vector2 prevDir = Vector2.zero; // Cached last movement
        private float prevRot; // Cached last look

        /// <summary>
        /// Keeps track of the velocity, based on a animation curve
        /// </summary>
        private float x;

        private float speedMultiplier = 1.0f;

        private bool locked;
        private bool stopped = true; // If the ship is not moving
        private bool dodgeFrame; // true if we can apply unclamped force, i.e. start or end of dodge

        partial void ServerStart() {
            //sm = _shipManager.shipMovement;
            //sds = _shipManager.sds;
            loadout = _shipManager.shipLoadout;
            movement = loadout.ShipMovement;
        }

        /// <summary>
        /// The server controls the movement of the ships. This is the code for it
        /// </summary>
        partial void ServerUpdate()
        {
            //_shipManager.rigidbody2D.rotation = GetRotation(targetAngle);
            //_shipManager.rigidbody2D.AddForce( movementVector * 10);
            //Debug.Log(targetAngle);
            
            var theta = GetRotation(targetAngle);
            _shipManager._rigidbody2D.rotation = theta;
            var vel = GetVelocity(movementVector);

            // WASD Relative to ship (feels icky to me idk)
            // For global relative just skip the rotation step use vel directly
            //var rotX = Mathf.Cos(theta * Mathf.Deg2Rad) * vel.x - Mathf.Sin(theta * Mathf.Deg2Rad) * vel.y;
            //var rotY = Mathf.Cos(theta * Mathf.Deg2Rad) * vel.y + Mathf.Sin(theta * Mathf.Deg2Rad) * vel.x;
            
            //var desiredVelocity = new Vector2(rotX, rotY);
            var desiredVelocity = vel;
            var currentVelocity = _shipManager._rigidbody2D.velocity;
            var deltaV = desiredVelocity - currentVelocity;
            var force = _shipManager._rigidbody2D.mass * (deltaV / Time.deltaTime);
            if (dodgeFrame) {
                _shipManager._rigidbody2D.AddForce(force);
                dodgeFrame = false;
            } else {
                _shipManager._rigidbody2D.AddForce(Vector2.ClampMagnitude(force, movement.maxForce));
            }
            
            
        }

        public void SetLocked(bool l) {
            locked = l;
        }
        
        public void SetSpeedMultiplier(float multiplier = 1.0f) {
            speedMultiplier = multiplier;
        }

        public void Dodge(Vector2 direction) {
            locked = true;
            //speedMultiplier = sds.speedMultiplier;
            prevDir = movementVector;
            dodgeFrame = true;
        }

        public void StopDodge() {
            locked = false;
            speedMultiplier = 1f;
            dodgeFrame = true;
            //prevDir = lockedOutDir;
        }

        public IEnumerator Fire() {
            while (true) {
                var hit = Physics2D.Raycast(transform.position + transform.up * 2, transform.up);
                if (hit.rigidbody) {
                    hit.rigidbody.AddForceAtPosition(transform.up * 10, hit.point, ForceMode2D.Impulse);
                }
                yield return new WaitForSeconds(.1f);
            }
        }
        
        private Vector2 GetVelocity(Vector2 movementDir) {

            if (locked) return stopped ? Vector2.zero : prevDir * movement.maxSpeed * speedMultiplier;
        
            if (movementDir != Vector2.zero) { 
                
                x += Time.deltaTime;
                x = Mathf.Clamp(x , 0, movement.accelTime);
                
                /* Old movement logic, might be good for a "drift"?
                var movDeltaAngle = Mathf.Clamp(Vector2.SignedAngle(prevDir, movementDir) * Mathf.Deg2Rad, -sm.turnSpeed * Time.deltaTime, sm.turnSpeed * Time.deltaTime);
                var curMovAngle = Mathf.Atan2(prevDir.y, prevDir.x);
                var newMovAngleDeg = curMovAngle + movDeltaAngle;
                */ 
                
                var newMovAngle = Vector2.MoveTowards(prevDir, movementDir, movement.turnSpeed * Time.deltaTime);
                
                prevDir = stopped
                    ? movementDir
                    : newMovAngle;
                stopped = false;
            } else {
                x -= Time.deltaTime;
                x = Mathf.Clamp(x, 0, movement.accelTime);
                if (x < float.Epsilon) {
                    stopped = true;
                }
            }

            var rawVel = prevDir * (movement.accelCurve.Evaluate(Mathf.Clamp(x / movement.accelTime, 0f, 1f)) * movement.maxSpeed);
            var correctionFactor = 
                Mathf.Lerp(
                    movement.backwardsSpeedFactor, 
                    1.0f, 
                    (
                        Vector2.Dot(
                            rawVel.normalized, 
                            new Vector2(Mathf.Cos((prevRot + 90f) * Mathf.Deg2Rad), Mathf.Sin((prevRot + 90f) * Mathf.Deg2Rad))
                        ) + 1
                    ) / 2
                );
            var targetVel = rawVel * correctionFactor * speedMultiplier;
            return targetVel;
        }
        
        private float GetRotation(float tAngle) {
        
            if (locked) return prevRot;
        
            // Get the mouse angle, do some interpolation with the current facing direction, rotate
        
            var newAngle = Mathf.MoveTowardsAngle(prevRot, tAngle,
                movement.maxTheta * 360 * Time.deltaTime);
            prevRot = newAngle;

            return newAngle;
        }

        /// <summary>
        /// Resets the previous known inertia
        /// </summary>
        public partial void ResetInertia()
        {
            x = 0;
            _shipManager._rigidbody2D.velocity = Vector2.zero;
        }
        
    }
}