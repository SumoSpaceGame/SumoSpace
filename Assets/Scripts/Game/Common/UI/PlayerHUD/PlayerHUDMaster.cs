using System.Collections;
using System.Collections.Generic;
using FishNet;
using Game.Common.Gameplay.Ship;
using Game.Common.Instances;
using Game.Common.Map;
using Game.Common.Settings;
using TMPro;
using UnityEngine;

public class PlayerHUDMaster : MonoBehaviour
{
    public MasterSettings settings;
    
    // TEMP way of doing this
    // Will restructure later

    public TextMeshProUGUI RemainingTimeHudItem;
    
    private ShipManager playerShip;
    // Start is called before the first frame update
    void Start()
    {
        if (InstanceFinder.IsServer)
        {
            this.enabled = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerShip == null)
        {
            playerShip = settings.GetPlayerShip();
            Debug.Log("Getting player ship " + playerShip);
        }

        if (playerShip == null) return;

        
        if (playerShip.mapTracker.currentWarningTimer > 0)
        {
            RemainingTimeHudItem.enabled = true;
            RemainingTimeHudItem.text = (Mathf.Clamp(playerShip.mapTracker.shipWarningSeconds.value - playerShip.mapTracker.currentWarningTimer, 0, float.PositiveInfinity)).ToString("0.00");
        }
        else
        {
            RemainingTimeHudItem.enabled = false;
        }
        
    }
}
