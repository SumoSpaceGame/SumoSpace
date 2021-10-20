using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Client.Gameplay.Abilities
{
    public abstract class ClientShipAbility : ScriptableObject, ICommandPerformer {
        private int ID;
        [FormerlySerializedAs("ship")] public ShipManager shipManager; // Ship this ability is bound to

        /*
     * If the ability is performed on a Player Ship, send this data over the server so everyone else sees it
     * Then, render out this ability regardless on the ship this is attached to
     */
        public void Perform() {
            if(shipManager.isPlayer) Execute();
            Render();
        }
    
        protected abstract void Execute();
    
        protected abstract void Render();

        //Command Handler
        //Dictionary<id, abilityClass>

        //Server & Client functions (Separate classes based on an interface)
        //public abstract void Receive(string data);

        public void Receive(ShipManager shipManager, string data)
        {
            throw new System.NotImplementedException();
        }

        public bool Receive(ShipManager manager, ICommandNetworker networker, CommandPacketData packetData)
        {
            throw new System.NotImplementedException();
        }

        public bool Perform(ShipManager shipManager, ICommandNetworker networker, params object[] arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}
