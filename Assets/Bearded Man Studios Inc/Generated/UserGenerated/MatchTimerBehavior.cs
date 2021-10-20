using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"uint\", \"long\"][\"uint\"][\"uint\", \"long\"][\"uint\"][\"uint\"][\"uint\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"timerID\", \"stopTime\"][\"timerID\"][\"timerID\", \"stopTime\"][\"timerID\"][\"timerID\"][\"timerID\"]]")]
	public abstract partial class MatchTimerBehavior : NetworkBehavior
	{
		public const byte RPC_START_TIMER_HANDLER = 0 + 5;
		public const byte RPC_PAUSE_TIMER_HANDLER = 1 + 5;
		public const byte RPC_RESUME_TIMER_HANDLER = 2 + 5;
		public const byte RPC_STOP_TIMER_HANDLER = 3 + 5;
		public const byte RPC_CREATE_CLIENT_TIMER_HANDLER = 4 + 5;
		public const byte RPC_DESTROY_CLIENT_TIMER_HANDLER = 5 + 5;
		
		public MatchTimerNetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (MatchTimerNetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("StartTimerRPCHandler", StartTimerRPCHandler, typeof(uint), typeof(long));
			networkObject.RegisterRpc("PauseTimerRPCHandler", PauseTimerRPCHandler, typeof(uint));
			networkObject.RegisterRpc("ResumeTimerRPCHandler", ResumeTimerRPCHandler, typeof(uint), typeof(long));
			networkObject.RegisterRpc("StopTimerRPCHandler", StopTimerRPCHandler, typeof(uint));
			networkObject.RegisterRpc("CreateClientTimerRPCHandler", CreateClientTimerRPCHandler, typeof(uint));
			networkObject.RegisterRpc("DestroyClientTimerRPCHandler", DestroyClientTimerRPCHandler, typeof(uint));

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
			Initialize(new MatchTimerNetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new MatchTimerNetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// uint timerID
		/// long stopTime
		/// </summary>
		public abstract void StartTimerRPCHandler(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint timerID
		/// </summary>
		public abstract void PauseTimerRPCHandler(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint timerID
		/// long stopTime
		/// </summary>
		public abstract void ResumeTimerRPCHandler(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint timerID
		/// </summary>
		public abstract void StopTimerRPCHandler(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint timerID
		/// </summary>
		public abstract void CreateClientTimerRPCHandler(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// uint timerID
		/// </summary>
		public abstract void DestroyClientTimerRPCHandler(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}