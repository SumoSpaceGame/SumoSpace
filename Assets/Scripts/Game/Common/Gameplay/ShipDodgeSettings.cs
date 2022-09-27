using System.Collections;
using Game.Common.Gameplay.Ship;
using UnityEngine;

[CreateAssetMenu(menuName = "Ship Movement/Dodge", fileName = "Dodge")]
public class ShipDodgeSettings : ScriptableObject {
    [SerializeField] private float duration;
    [SerializeField] public float speedMultiplier;

    public IEnumerator FinishDodge(ShipManager shipManager)
    {
        yield return new WaitForSeconds(duration);

        shipManager.EnableColliders();
        shipManager.shipController.StopDodge();

        //networker.SendData(CommandPacketData.Create(new byte[]{1}), (int)CommandType.AGILITY_DODGE, shipManager.playerMatchID);
    }
}