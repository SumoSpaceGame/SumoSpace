using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(global::Game.Common.Gameplay.Ship.ShipManager))]
    public partial class ShipController : MonoBehaviour
    {
        public ShipMovement sm;

        [HideInInspector]
        public Vector2 movementVector;
        [HideInInspector]
        public float targetAngle;
        
        private Vector2 prevDir = Vector2.zero; // Cached last movement
        private float prevRot; // Cached last look

        private float x;

        private bool locked;
        private bool stopped = true; // If the ship is not moving

        partial void ServerStart() {
            sm = _shipManager.shipMovement;
        }

        /// <summary>
        /// The server controls the movement of the ships. This is the code for it
        /// </summary>
        partial void ServerUpdate()
        {
            //_shipManager.rigidbody2D.rotation = GetRotation(targetAngle);
            //_shipManager.rigidbody2D.AddForce( movementVector * 10);

            
            _shipManager._rigidbody2D.rotation = GetRotation(targetAngle);
            
            var desiredVelocity = GetVelocity(movementVector);
            var currentVelocity = _shipManager._rigidbody2D.velocity;
            var deltaV = desiredVelocity - currentVelocity;
            var force = _shipManager._rigidbody2D.mass * (deltaV / Time.deltaTime);
            _shipManager._rigidbody2D.AddForce(force);
            
        }
        
        public void SetLocked(bool l) {
            locked = l;
        }
        
        private Vector2 GetVelocity(Vector2 movementDir) {

            if (locked) return prevDir;
        
            if (movementDir != Vector2.zero) { 
                stopped = false;
                x += Time.deltaTime;
                x = Mathf.Clamp(x , 0, sm.accelTime);
                // Interpolate between 
                var movDeltaAngle = Mathf.Clamp(Vector2.SignedAngle(prevDir, movementDir) * Mathf.Deg2Rad, -sm.turnRadius * Time.deltaTime, sm.turnRadius * Time.deltaTime);
                var curMovAngle = Mathf.Atan2(prevDir.y, prevDir.x);
                var newMovAngle = curMovAngle + movDeltaAngle;
                prevDir = stopped ? movementDir : new Vector2(Mathf.Cos(newMovAngle), Mathf.Sin(newMovAngle));
            } else {
                x -= Time.deltaTime;
                x = Mathf.Clamp(x, 0, sm.accelTime);
                if (x < float.Epsilon) {
                    stopped = true;
                }
            }

            var rawVel = prevDir * (sm.accelCurve.Evaluate(Mathf.Clamp(x / sm.accelTime, 0f, 1f)) * sm.maxSpeed);
            var correctionFactor = 
                Mathf.Lerp(
                    sm.backwardsSpeedFactor, 
                    1.0f, 
                    (
                        Vector2.Dot(
                            rawVel.normalized, 
                            new Vector2(Mathf.Cos((prevRot + 90f) * Mathf.Deg2Rad), Mathf.Sin((prevRot + 90f) * Mathf.Deg2Rad))
                        ) + 1
                    ) / 2
                );
            var targetVel = rawVel * correctionFactor;
            return targetVel;
        }
        
        private float GetRotation(float tAngle) {
        
            if (locked) return prevRot;
        
            // Get the mouse angle, do some interpolation with the current facing direction, rotate
        
            var newAngle = Mathf.MoveTowardsAngle(prevRot, tAngle,
                sm.maxTheta * 360 * Time.deltaTime);
            prevRot = newAngle;
            
            return newAngle;
        }
    }
}