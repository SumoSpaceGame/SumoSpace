using System;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Instances;
using UnityEngine;

[RequireComponent(typeof(ShipSpawner))]
public partial class AgentNetworkManager : AgentManagerBehavior, IGamePersistantInstance
{

    public GameMatchSettings gameMatchSettings;
    
    public Dictionary<int, Ship> _playerShips = new Dictionary<int, Ship>();


    private ShipSpawner _shipSpawner = new ShipSpawner();
    
    public void Awake()
    {
        MainPersistantInstances.Add(this);
        _shipSpawner = GetComponent<ShipSpawner>();
    }

    private void OnDestroy()
    {
        MainPersistantInstances.Remove<AgentNetworkManager>();
    }


    public override void UpdateMovement(RpcArgs args)
    {
        
    }
    
    public override void CreateShip(RpcArgs args)
    {
        if (networkObject.IsServer)
        {
            // TODO: Makemore compatiable elsewhere
            Debug.LogError("Server create ship called, server should not create any ships through RSVP");
            return;
        }
        
        ushort shipPlayerMatchID = args.GetAt<ushort>(0);
        int shipType = args.GetAt<int>(1); // TODO: Temp
        
        _shipSpawner.SpawnShip(shipPlayerMatchID, shipType, gameMatchSettings);
    }
    
    partial void ClientUpdateMovement();
}
