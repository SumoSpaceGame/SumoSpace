using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(global::Game.Common.Gameplay.Ship.ShipManager))]
    public partial class ShipController : MonoBehaviour
    {
        public ShipMovement shipMovement;

        [HideInInspector]
        public Vector2 movementVector;
        [HideInInspector]
        public float targetAngle;

        partial void ServerStart()
        {
        }


        /// <summary>
        /// The server controls the movement of the ships. This is the code for it
        /// </summary>
        partial void ServerUpdate()
        {
            _shipManager._rigidbody2D.rotation = targetAngle;
            _shipManager._rigidbody2D.AddForce( movementVector * 10);

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