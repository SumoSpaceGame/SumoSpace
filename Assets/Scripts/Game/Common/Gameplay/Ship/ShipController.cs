using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(global::Ship))]
    public partial class ShipController : MonoBehaviour
    {

        private global::Ship ship;
    
        private void Awake()
        {
            ship = GetComponent<global::Ship>();
            ship.shipController = this;
        }


        public void Update()
        {
            if(ship.isServer) ServerUpdate();
        }

        partial void ServerUpdate();
    }
}
