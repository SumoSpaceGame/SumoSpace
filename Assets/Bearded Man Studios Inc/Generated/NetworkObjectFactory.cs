using BeardedManStudios.Forge.Networking.Frame;
using System;
using MainThreadManager = BeardedManStudios.Forge.Networking.Unity.MainThreadManager;

namespace BeardedManStudios.Forge.Networking.Generated
{
	public partial class NetworkObjectFactory : NetworkObjectFactoryBase
	{
		public override void NetworkCreateObject(NetWorker networker, int identity, uint id, FrameStream frame, Action<NetworkObject> callback)
		{
			if (networker.IsServer)
			{
				if (frame.Sender != null && frame.Sender != networker.Me)
				{
					if (!ValidateCreateRequest(networker, identity, id, frame))
						return;
				}
			}
			
			bool availableCallback = false;
			NetworkObject obj = null;
			MainThreadManager.Run(() =>
			{
				switch (identity)
				{
					case AgentInputNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new AgentInputNetworkObject(networker, id, frame);
						break;
					case AgentManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new AgentManagerNetworkObject(networker, id, frame);
						break;
					case AgentMovementNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new AgentMovementNetworkObject(networker, id, frame);
						break;
					case GameManagerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new GameManagerNetworkObject(networker, id, frame);
						break;
					case GamePhaseNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new GamePhaseNetworkObject(networker, id, frame);
						break;
					case InputLayerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new InputLayerNetworkObject(networker, id, frame);
						break;
					case MatchTimerNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new MatchTimerNetworkObject(networker, id, frame);
						break;
					case NetBibleNetworkObject.IDENTITY:
						availableCallback = true;
						obj = new NetBibleNetworkObject(networker, id, frame);
						break;
				}

				if (!availableCallback)
					base.NetworkCreateObject(networker, identity, id, frame, callback);
				else if (callback != null)
					callback(obj);
			});
		}

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}