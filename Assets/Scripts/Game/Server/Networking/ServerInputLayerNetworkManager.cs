using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using Game.Common.Instances;
using UnityEngine;

namespace Game.Common.Networking
{
    public partial class InputLayerNetworkManager : InputLayerBehavior, IGamePersistantInstance
    {
        partial void ServerMovementUpdate(RpcArgs args)
        {
            var movementVec = args.GetAt<Vector2>(0);
            var rotationVec = args.GetAt<float>(1);
            var playerID = args.Info.SendingPlayer.NetworkId;
            MainThreadManager.Run(() =>
            {
                var ship = masterSettings.GetShip(playerID);

                if (ship == null)
                {
                    Debug.LogError("Tried to get a ship from a player that does not have a ship");
                    return;
                }
                
                ship.shipController.movementVector = movementVec;
                ship.shipController.targetAngle = rotationVec;
            });
        }
    }
}