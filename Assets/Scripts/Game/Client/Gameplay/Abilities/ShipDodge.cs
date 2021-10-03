using UnityEngine;

[CreateAssetMenu(menuName = "Ship Abilities/Ship Dodge")]
public class ShipDodge : ShipAbility {
    [SerializeField] private ShipMovement shipMovement;

    public override void Perform() {
        shipMovement.SetLocked(true);
        ship.invulnerable = true;
        
        
    }
    
    
}
