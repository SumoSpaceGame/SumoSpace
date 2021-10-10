using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;

public partial class AgentNetworkManager : AgentManagerBehavior
{

    public GameMatchSettings gameMatchSettings;

    public Dictionary<int, Ship> playerShips = new Dictionary<int, Ship>();
    
    public override void UpdateMovement(RpcArgs args)
    {
        
    }

    public override void CreateShip(RpcArgs args)
    {
        ushort shipPlayerMatchID = args.GetAt<ushort>(0);
        int shipType = args.GetAt<int>(1); // TODO: Temp
        
    }
    
    partial void ClientUpdateMovement();
}
