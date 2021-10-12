using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : AgentMovementBehavior
    {
        partial void ServerUpdate()
        {
            networkObject.position = attachedShip.transform.position;
            networkObject.rotation = attachedShip.transform.eulerAngles.z;
        }

        partial void ServerRequestShipSpawnData(RpcArgs args)
        {
            // TODO: Ship sync

            var requestData = new RequestData();
            requestData.clientOwner = attachedShip.playerMatchID;
            
            networkObject.SendRpc(args.Info.SendingPlayer, RPC_REQUEST_SHIP_SPAWN_DATA, requestData.Serialize());
        }
    }
}