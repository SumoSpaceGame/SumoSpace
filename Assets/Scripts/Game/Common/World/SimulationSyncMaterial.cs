using System.Collections;
using System.Collections.Generic;
using FishNet;
using Game.Common.Settings;
using UnityEngine;

public class SimulationSyncMaterial : MonoBehaviour
{
    public MasterSettings masterSettings;

    public Renderer mapRenderer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (InstanceFinder.IsServer) return;
        var ship = masterSettings.GetPlayerShip();

        if (ship)
        {
            var pos = ship.GetWorldPosition();
            mapRenderer.material.SetVector("Vector3_3f6de148a2f34818a36a3487773b1052", new Vector4(pos.x, pos.y, pos.z));
        }
    }
}
