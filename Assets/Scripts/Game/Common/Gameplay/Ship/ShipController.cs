using System;
using UnityEngine;

namespace Game.Common.Gameplay.Ship
{
    [RequireComponent(typeof(global::Game.Common.Gameplay.Ship.ShipManager))]
    public partial class ShipController : MonoBehaviour
    {

        private global::Game.Common.Gameplay.Ship.ShipManager _shipManager;
    
        private void Awake()
        {
            _shipManager = GetComponent<global::Game.Common.Gameplay.Ship.ShipManager>();
            _shipManager.shipController = this;
        }

        private void Start()
        {
            if (_shipManager.isServer) ServerStart();
        }


        public void Update()
        {
            if(_shipManager.isServer) ServerUpdate();
        }

        partial void ServerStart();
        partial void ServerUpdate();
    }
}
