using BeardedManStudios.Forge.Networking.Generated;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Unity
{
	public partial class NetworkManager : MonoBehaviour
	{
		public delegate void InstantiateEvent(INetworkBehavior unityGameObject, NetworkObject obj);
		public event InstantiateEvent objectInitialized;
		protected BMSByte metadata = new BMSByte();

		public GameObject[] AgentInputNetworkObject = null;
		public GameObject[] AgentManagerNetworkObject = null;
		public GameObject[] AgentMovementNetworkObject = null;
		public GameObject[] GameManagerNetworkObject = null;
		public GameObject[] GamePhaseNetworkObject = null;
		public GameObject[] InputLayerNetworkObject = null;
		public GameObject[] MatchTimerNetworkObject = null;
		public GameObject[] NetBibleNetworkObject = null;

		protected virtual void SetupObjectCreatedEvent()
		{
			Networker.objectCreated += CaptureObjects;
		}

		protected virtual void OnDestroy()
		{
		    if (Networker != null)
				Networker.objectCreated -= CaptureObjects;
		}
		
		private void CaptureObjects(NetworkObject obj)
		{
			if (obj.CreateCode < 0)
				return;
				
			if (obj is AgentInputNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (AgentInputNetworkObject.Length > 0 && AgentInputNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(AgentInputNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<AgentInputBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is AgentManagerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (AgentManagerNetworkObject.Length > 0 && AgentManagerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(AgentManagerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<AgentManagerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is AgentMovementNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (AgentMovementNetworkObject.Length > 0 && AgentMovementNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(AgentMovementNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<AgentMovementBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is GameManagerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (GameManagerNetworkObject.Length > 0 && GameManagerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(GameManagerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<GameManagerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is GamePhaseNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (GamePhaseNetworkObject.Length > 0 && GamePhaseNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(GamePhaseNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<GamePhaseBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is InputLayerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (InputLayerNetworkObject.Length > 0 && InputLayerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(InputLayerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<InputLayerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is MatchTimerNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (MatchTimerNetworkObject.Length > 0 && MatchTimerNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(MatchTimerNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<MatchTimerBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
			else if (obj is NetBibleNetworkObject)
			{
				MainThreadManager.Run(() =>
				{
					NetworkBehavior newObj = null;
					if (!NetworkBehavior.skipAttachIds.TryGetValue(obj.NetworkId, out newObj))
					{
						if (NetBibleNetworkObject.Length > 0 && NetBibleNetworkObject[obj.CreateCode] != null)
						{
							var go = Instantiate(NetBibleNetworkObject[obj.CreateCode]);
							newObj = go.GetComponent<NetBibleBehavior>();
						}
					}

					if (newObj == null)
						return;
						
					newObj.Initialize(obj);

					if (objectInitialized != null)
						objectInitialized(newObj, obj);
				});
			}
		}

		protected virtual void InitializedObject(INetworkBehavior behavior, NetworkObject obj)
		{
			if (objectInitialized != null)
				objectInitialized(behavior, obj);

			obj.pendingInitialized -= InitializedObject;
		}

		[Obsolete("Use InstantiateAgentInput instead, its shorter and easier to type out ;)")]
		public AgentInputBehavior InstantiateAgentInputNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(AgentInputNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentInputBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<AgentInputBehavior>().networkObject = (AgentInputNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateAgentManager instead, its shorter and easier to type out ;)")]
		public AgentManagerBehavior InstantiateAgentManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(AgentManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentManagerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<AgentManagerBehavior>().networkObject = (AgentManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateAgentMovement instead, its shorter and easier to type out ;)")]
		public AgentMovementBehavior InstantiateAgentMovementNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(AgentMovementNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentMovementBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<AgentMovementBehavior>().networkObject = (AgentMovementNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateGameManager instead, its shorter and easier to type out ;)")]
		public GameManagerBehavior InstantiateGameManagerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(GameManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<GameManagerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<GameManagerBehavior>().networkObject = (GameManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateGamePhase instead, its shorter and easier to type out ;)")]
		public GamePhaseBehavior InstantiateGamePhaseNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(GamePhaseNetworkObject[index]);
			var netBehavior = go.GetComponent<GamePhaseBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<GamePhaseBehavior>().networkObject = (GamePhaseNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateInputLayer instead, its shorter and easier to type out ;)")]
		public InputLayerBehavior InstantiateInputLayerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(InputLayerNetworkObject[index]);
			var netBehavior = go.GetComponent<InputLayerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<InputLayerBehavior>().networkObject = (InputLayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateMatchTimer instead, its shorter and easier to type out ;)")]
		public MatchTimerBehavior InstantiateMatchTimerNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(MatchTimerNetworkObject[index]);
			var netBehavior = go.GetComponent<MatchTimerBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<MatchTimerBehavior>().networkObject = (MatchTimerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		[Obsolete("Use InstantiateNetBible instead, its shorter and easier to type out ;)")]
		public NetBibleBehavior InstantiateNetBibleNetworkObject(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			var go = Instantiate(NetBibleNetworkObject[index]);
			var netBehavior = go.GetComponent<NetBibleBehavior>();
			var obj = netBehavior.CreateNetworkObject(Networker, index);
			go.GetComponent<NetBibleBehavior>().networkObject = (NetBibleNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}

		/// <summary>
		/// Instantiate an instance of AgentInput
		/// </summary>
		/// <returns>
		/// A local instance of AgentInputBehavior
		/// </returns>
		/// <param name="index">The index of the AgentInput prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public AgentInputBehavior InstantiateAgentInput(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (AgentInputNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: AgentInput. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(AgentInputNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentInputBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<AgentInputBehavior>().networkObject = (AgentInputNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of AgentManager
		/// </summary>
		/// <returns>
		/// A local instance of AgentManagerBehavior
		/// </returns>
		/// <param name="index">The index of the AgentManager prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public AgentManagerBehavior InstantiateAgentManager(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (AgentManagerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: AgentManager. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(AgentManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentManagerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<AgentManagerBehavior>().networkObject = (AgentManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of AgentMovement
		/// </summary>
		/// <returns>
		/// A local instance of AgentMovementBehavior
		/// </returns>
		/// <param name="index">The index of the AgentMovement prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public AgentMovementBehavior InstantiateAgentMovement(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (AgentMovementNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: AgentMovement. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(AgentMovementNetworkObject[index]);
			var netBehavior = go.GetComponent<AgentMovementBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<AgentMovementBehavior>().networkObject = (AgentMovementNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of GameManager
		/// </summary>
		/// <returns>
		/// A local instance of GameManagerBehavior
		/// </returns>
		/// <param name="index">The index of the GameManager prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public GameManagerBehavior InstantiateGameManager(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (GameManagerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: GameManager. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(GameManagerNetworkObject[index]);
			var netBehavior = go.GetComponent<GameManagerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<GameManagerBehavior>().networkObject = (GameManagerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of GamePhase
		/// </summary>
		/// <returns>
		/// A local instance of GamePhaseBehavior
		/// </returns>
		/// <param name="index">The index of the GamePhase prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public GamePhaseBehavior InstantiateGamePhase(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (GamePhaseNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: GamePhase. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(GamePhaseNetworkObject[index]);
			var netBehavior = go.GetComponent<GamePhaseBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<GamePhaseBehavior>().networkObject = (GamePhaseNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of InputLayer
		/// </summary>
		/// <returns>
		/// A local instance of InputLayerBehavior
		/// </returns>
		/// <param name="index">The index of the InputLayer prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public InputLayerBehavior InstantiateInputLayer(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (InputLayerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: InputLayer. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(InputLayerNetworkObject[index]);
			var netBehavior = go.GetComponent<InputLayerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<InputLayerBehavior>().networkObject = (InputLayerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of MatchTimer
		/// </summary>
		/// <returns>
		/// A local instance of MatchTimerBehavior
		/// </returns>
		/// <param name="index">The index of the MatchTimer prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public MatchTimerBehavior InstantiateMatchTimer(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (MatchTimerNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: MatchTimer. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(MatchTimerNetworkObject[index]);
			var netBehavior = go.GetComponent<MatchTimerBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<MatchTimerBehavior>().networkObject = (MatchTimerNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
		/// <summary>
		/// Instantiate an instance of NetBible
		/// </summary>
		/// <returns>
		/// A local instance of NetBibleBehavior
		/// </returns>
		/// <param name="index">The index of the NetBible prefab in the NetworkManager to Instantiate</param>
		/// <param name="position">Optional parameter which defines the position of the created GameObject</param>
		/// <param name="rotation">Optional parameter which defines the rotation of the created GameObject</param>
		/// <param name="sendTransform">Optional Parameter to send transform data to other connected clients on Instantiation</param>
		public NetBibleBehavior InstantiateNetBible(int index = 0, Vector3? position = null, Quaternion? rotation = null, bool sendTransform = true)
		{
			if (NetBibleNetworkObject.Length <= index)
			{
				Debug.Log("Prefab(s) missing for: NetBible. Add them at the NetworkManager prefab.");
				return null;
			}
			
			var go = Instantiate(NetBibleNetworkObject[index]);
			var netBehavior = go.GetComponent<NetBibleBehavior>();

			NetworkObject obj = null;
			if (!sendTransform && position == null && rotation == null)
				obj = netBehavior.CreateNetworkObject(Networker, index);
			else
			{
				metadata.Clear();

				if (position == null && rotation == null)
				{
					byte transformFlags = 0x1 | 0x2;
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);
					ObjectMapper.Instance.MapBytes(metadata, go.transform.position, go.transform.rotation);
				}
				else
				{
					byte transformFlags = 0x0;
					transformFlags |= (byte)(position != null ? 0x1 : 0x0);
					transformFlags |= (byte)(rotation != null ? 0x2 : 0x0);
					ObjectMapper.Instance.MapBytes(metadata, transformFlags);

					if (position != null)
						ObjectMapper.Instance.MapBytes(metadata, position.Value);

					if (rotation != null)
						ObjectMapper.Instance.MapBytes(metadata, rotation.Value);
				}

				obj = netBehavior.CreateNetworkObject(Networker, index, metadata.CompressBytes());
			}

			go.GetComponent<NetBibleBehavior>().networkObject = (NetBibleNetworkObject)obj;

			FinalizeInitialization(go, netBehavior, obj, position, rotation, sendTransform);
			
			return netBehavior;
		}
	}
}
