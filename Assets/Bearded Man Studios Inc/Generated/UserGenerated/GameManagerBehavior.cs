using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"string\"][\"string\"][\"string\"][\"uint\", \"ushort\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"data\"][\"clientID\"][\"UserID\"][\"networkID\", \"matchID\"]]")]
	public abstract partial class GameManagerBehavior : NetworkBehavior
	{
		public const byte RPC_SYNC_MATCH_SETTINGS = 0 + 5;
		public const byte RPC_REQUEST_CLIENT_I_D = 1 + 5;
		public const byte RPC_REQUEST_SERVER_JOIN = 2 + 5;
		public const byte RPC_UPDATE_PLAYER_NETWORK_I_D = 3 + 5;
		
		public GameManagerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (GameManagerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("SyncMatchSettings", SyncMatchSettings, typeof(string));
			networkObject.RegisterRpc("RequestClientID", RequestClientID, typeof(string));
			networkObject.RegisterRpc("RequestServerJoin", RequestServerJoin, typeof(string));
			networkObject.RegisterRpc("UpdatePlayerNetworkID", UpdatePlayerNetworkID, typeof(uint), typeof(ushort));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new GameManagerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new GameManagerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// string data
		/// </summary>
		public abstract void SyncMatchSettings(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// string clientID
		/// </summary>
		public abstract void RequestClientID(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// string UserID
		/// </summary>
		public abstract void RequestServerJoin(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint networkID
		/// ushort matchID
		/// </summary>
		public abstract void UpdatePlayerNetworkID(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}