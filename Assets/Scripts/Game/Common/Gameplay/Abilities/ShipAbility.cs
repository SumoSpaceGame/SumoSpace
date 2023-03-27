﻿using Game.Common.Gameplay.Commands;
using Game.Common.Gameplay.Ship;
using UnityEngine;

//[CreateAssetMenu(menuName = "Ship Abilities", fileName = "New Ability", order = 1)]
namespace Game.Common.Gameplay.Abilities
{
    public abstract class ShipAbility : ScriptableObject
    {

        public void Init()
        {
            this.executeCommand = "";
            this.stopCommand = "";
        }
        
        protected abstract void OnInit();

        [SerializeField] protected float cooldown;
        // The command that triggers execution of this ability
        protected string executeCommand = "";
        // The command that triggers this ability to stop
        protected string stopCommand = "";
    
        [SerializeField] protected GameObject clientBehaviour;
        [SerializeField] protected GameObject serverBehaviour;

        [SerializeField] protected ShipLoadout.AbilityType slot;
        public string ExecuteCommand => executeCommand;
        public string StopCommand => stopCommand;
        public float Cooldown => cooldown;
        public GameObject ClientBehaviour => clientBehaviour;
        public GameObject ServerBehaviour => serverBehaviour;
    
        public ShipLoadout.AbilityType Slot => slot;

        // Optional function for resolving network lag visual issues (especially on coroutine dependent functions)
        public virtual void QuickExecute(ShipManager shipManager, bool isServer) {
            if (!isServer) {
                shipManager.behaviours[(int)slot].QuickExecute();
            }
        }

        // Execute the ability
        public virtual void Execute(ShipManager shipManager, bool isServer) {
            shipManager.behaviours[(int)slot].Execute();
        }
    
        // Stop executing the ability
        public virtual void Stop(ShipManager shipManager, bool isServer) {
            shipManager.behaviours[(int)slot].Stop();
        }

        // Add the corresponding behaviour to the given game object
        public void AddBehaviour(ShipManager shipManager, ShipLoadout.AbilityType type) {
            if (shipManager.isServer) {
                shipManager.behaviours[(int)type] = Instantiate(serverBehaviour, shipManager.transform).GetComponent<AbilityBehaviourComponent>();
            } else {
                shipManager.behaviours[(int)type] = Instantiate(clientBehaviour, shipManager.simulationObject.representative.transform).GetComponent<AbilityBehaviourComponent>();
            }
            shipManager.behaviours[(int)type].Init(shipManager);
        }
        
        #if UNITY_EDITOR

        public void SetClientBehaviour(GameObject cb)
        {
            clientBehaviour = cb;
        }
        
        public void SetServerBehaviour(GameObject sb)
        {
            serverBehaviour = sb;
        }

        #endif
    }
}
