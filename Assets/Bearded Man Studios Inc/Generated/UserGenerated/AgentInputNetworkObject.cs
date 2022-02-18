using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0,0]")]
	public partial class AgentInputNetworkObject : NetworkObject
	{
		public const int IDENTITY = 1;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector2 _inputDirection;
		public event FieldEvent<Vector2> inputDirectionChanged;
		public InterpolateVector2 inputDirectionInterpolation = new InterpolateVector2() { LerpT = 0f, Enabled = false };
		public Vector2 inputDirection
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
		[ForgeGeneratedField]
		private float _inputRotation;
		public event FieldEvent<float> inputRotationChanged;
		public InterpolateFloat inputRotationInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float inputRotation
		{
			get { return _inputRotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_inputRotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_inputRotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetinputRotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_inputRotation(ulong timestep)
		{
			if (inputRotationChanged != null) inputRotationChanged(_inputRotation, timestep);
			if (fieldAltered != null) fieldAltered("inputRotation", _inputRotation, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			inputDirectionInterpolation.current = inputDirectionInterpolation.target;
			inputRotationInterpolation.current = inputRotationInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _inputDirection);
			UnityObjectMapper.Instance.MapBytes(data, _inputRotation);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_inputDirection = UnityObjectMapper.Instance.Map<Vector2>(payload);
			inputDirectionInterpolation.current = _inputDirection;
			inputDirectionInterpolation.target = _inputDirection;
			RunChange_inputDirection(timestep);
			_inputRotation = UnityObjectMapper.Instance.Map<float>(payload);
			inputRotationInterpolation.current = _inputRotation;
			inputRotationInterpolation.target = _inputRotation;
			RunChange_inputRotation(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _inputDirection);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _inputRotation);

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
					inputDirectionInterpolation.target = UnityObjectMapper.Instance.Map<Vector2>(data);
					inputDirectionInterpolation.Timestep = timestep;
				}
				else
				{
					_inputDirection = UnityObjectMapper.Instance.Map<Vector2>(data);
					RunChange_inputDirection(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (inputRotationInterpolation.Enabled)
				{
					inputRotationInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					inputRotationInterpolation.Timestep = timestep;
				}
				else
				{
					_inputRotation = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_inputRotation(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (inputDirectionInterpolation.Enabled && !inputDirectionInterpolation.current.UnityNear(inputDirectionInterpolation.target, 0.0015f))
			{
				_inputDirection = (Vector2)inputDirectionInterpolation.Interpolate();
				//RunChange_inputDirection(inputDirectionInterpolation.Timestep);
			}
			if (inputRotationInterpolation.Enabled && !inputRotationInterpolation.current.UnityNear(inputRotationInterpolation.target, 0.0015f))
			{
				_inputRotation = (float)inputRotationInterpolation.Interpolate();
				//RunChange_inputRotation(inputRotationInterpolation.Timestep);
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
