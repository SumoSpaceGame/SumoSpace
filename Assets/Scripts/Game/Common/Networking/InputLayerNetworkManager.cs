using System;
using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using Game.Common.Instances;
using UnityEngine;


public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
{
    private void Awake()
    {
        MainPersistantInstances.Add(this);
        DontDestroyOnLoad(this);
    }

    public override void CommandUpdate(RpcArgs args)
    {
        throw new NotImplementedException(); //Hadle COmmand
    }

    public override void MovementUpdate(RpcArgs args)
    {  
        
    }

    private void OnDestroy()
    {
        MainPersistantInstances.Remove<InputLayerNetworkManager>();
    }
}
