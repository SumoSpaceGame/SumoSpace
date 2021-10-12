
// ReSharper disable CheckNamespace

using BeardedManStudios.Forge.Networking.Frame;

namespace BeardedManStudios.Forge.Networking.Generated
{
    public partial class NetworkObjectFactory {
        
        /// <summary>
        /// Disables client side instantiation. This is only called on server side when a client instantiates.
        /// </summary>
        /// <param name="networker"></param>
        /// <param name="identity"></param>
        /// <param name="id"></param>
        /// <param name="frame"></param>
        /// <returns></returns>
        protected override bool ValidateCreateRequest(NetWorker networker, int identity, uint id, FrameStream frame)
        {
            return false;
        }
        
        
    }
}