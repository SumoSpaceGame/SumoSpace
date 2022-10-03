

using FishNet.Connection;
using FishNet.Object;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class AgentMovementNetworkManager : NetworkBehaviour
    {
        partial void ServerUpdate()
        {
            this.transform.position = attachedShipManager.transform.position;
            this.transform.eulerAngles = new Vector3(0,0, attachedShipManager.transform.eulerAngles.z);
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