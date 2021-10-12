using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(global::Ship))]
    public partial class ShipController : MonoBehaviour
    {
        public ShipMovement shipMovement;

        [HideInInspector]
        public Vector2 movementVector;
        [HideInInspector]
        public float targetAngle;

        /// <summary>
        /// The server controls the movement of the ships. This is the code for it
        /// </summary>
        partial void ServerUpdate()
        {
            ship.rigidbody2D.rotation = targetAngle;
            ship.rigidbody2D.AddForce( movementVector * 10);

            /**
            ship.rigidbody2D.rotation = shipMovement.GetRotation(targetAngle);
            
            var desiredVelocity = shipMovement.GetVelocity(movementVector);
            var currentVelocity = ship.rigidbody2D.velocity;
            var deltaV = desiredVelocity - currentVelocity;
            var force = ship.rigidbody2D.mass * (deltaV / Time.deltaTime);
            ship.rigidbody2D.AddForce(force);
            **/
        }
    }
}