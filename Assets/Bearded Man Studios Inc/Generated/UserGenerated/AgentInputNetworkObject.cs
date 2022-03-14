using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0]")]
	public partial class AgentInputNetworkObject : NetworkObject
	{
		public const int IDENTITY = 1;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _inputDirection;
		public event FieldEvent<Vector3> inputDirectionChanged;
		public InterpolateVector3 inputDirectionInterpolation = new InterpolateVector3() { LerpT = 0f, Enabled = false };
		public Vector3 inputDirection
		{
			get { return _inputDirection; }
			set
			{
				// Don't do anything if the value is the same
				if (_inputDirection == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_inputDirection = value;
				hasDirtyFields = true;
			}
		}

		public void SetinputDirectionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_inputDirection(ulong timestep)
		{
			if (inputDirectionChanged != null) inputDirectionChanged(_inputDirection, timestep);
			if (fieldAltered != null) fieldAltered("inputDirection", _inputDirection, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			inputDirectionInterpolation.current = inputDirectionInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _inputDirection);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_inputDirection = UnityObjectMapper.Instance.Map<Vector3>(payload);
			inputDirectionInterpolation.current = _inputDirection;
			inputDirectionInterpolation.target = _inputDirection;
			RunChange_inputDirection(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _inputDirection);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (inputDirectionInterpolation.Enabled)
				{
					inputDirectionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					inputDirectionInterpolation.Timestep = timestep;
				}
				else
				{
					_inputDirection = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_inputDirection(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (inputDirectionInterpolation.Enabled && !inputDirectionInterpolation.current.UnityNear(inputDirectionInterpolation.target, 0.0015f))
			{
				_inputDirection = (Vector3)inputDirectionInterpolation.Interpolate();
				//RunChange_inputDirection(inputDirectionInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public AgentInputNetworkObject() : base() { Initialize(); }
		public AgentInputNetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public AgentInputNetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
