

using FishNet.Connection;
using FishNet.Object;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : NetworkBehaviour
    {
        partial void ServerUpdate()
        {
            //networkObject.position = attachedShipManager.transform.position;
            //networkObject.rotation = attachedShipManager.transform.eulerAngles.z;
        }

        /// <summary>
        /// When the client wants data of the ship, which should happen only on spawn, the server will send it to the player.
        /// Data should encompass all required data to spawn the ship in one to one.
        /// </summary>
        /// <param name="args"></param>
        partial void ServerRequestShipSpawnData(NetworkConnection conn = null)
        {
            if (conn == null) return;
            
            var requestData = new RequestData();
            requestData.clientOwner = attachedShipManager.playerMatchID;
            
            ClientRequestShipSpawnData(conn, requestData);


        }
    }
}