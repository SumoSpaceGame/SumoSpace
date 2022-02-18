using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : AgentMovementBehavior
    {
        partial void ServerUpdate()
        {
            networkObject.position = attachedShipManager.transform.position;
            networkObject.rotation = attachedShipManager.transform.eulerAngles.z;
        }

        /// <summary>
        /// When the client wants data of the ship, which should happen only on spawn, the server will send it to the player.
        /// Data should encompass all required data to spawn the ship in one to one.
        /// </summary>
        /// <param name="args"></param>
        partial void ServerRequestShipSpawnData(RpcArgs args)
        {
            // TODO: Ship sync

            var requestData = new RequestData();
            requestData.clientOwner = attachedShipManager.playerMatchID;
            
            networkObject.SendRpc(args.Info.SendingPlayer, RPC_REQUEST_SHIP_SPAWN_DATA, requestData.Serialize());
            
            


        }
    }
}