using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class TagDecorator : ProtoDecoratorBase, IProtoSerializer, IProtoTypeSerializer
	{
		private readonly bool strict;

		private readonly int fieldNumber;

		private readonly WireType wireType;

		public override Type ExpectedType
		{
			get
			{
				return Tail.ExpectedType;
			}
		}

		public override bool RequiresOldValue
		{
			get
			{
				return Tail.RequiresOldValue;
			}
		}

		public override bool ReturnsValue
		{
			get
			{
				return Tail.ReturnsValue;
			}
		}

		private bool NeedsHint
		{
			get
			{
				return (wireType & (WireType)(-8)) != 0;
			}
		}

		public TagDecorator(int fieldNumber, WireType wireType, bool strict, IProtoSerializer tail)
			: base(tail)
		{
			this.fieldNumber = fieldNumber;
			this.wireType = wireType;
			this.strict = strict;
		}

		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			IProtoTypeSerializer protoTypeSerializer = Tail as IProtoTypeSerializer;
			return protoTypeSerializer != null && protoTypeSerializer.HasCallbacks(callbackType);
		}

		public bool CanCreateInstance()
		{
			IProtoTypeSerializer protoTypeSerializer = Tail as IProtoTypeSerializer;
			return protoTypeSerializer != null && protoTypeSerializer.CanCreateInstance();
		}

		public object CreateInstance(ProtoReader source)
		{
			return ((IProtoTypeSerializer)Tail).CreateInstance(source);
		}

		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			IProtoTypeSerializer protoTypeSerializer = Tail as IProtoTypeSerializer;
			if (protoTypeSerializer != null)
			{
				protoTypeSerializer.Callback(value, callbackType, context);
			}
		}

		public override object Read(object value, ProtoReader source)
		{
			if (strict)
			{
				source.Assert(wireType);
			}
			else if (NeedsHint)
			{
				source.Hint(wireType);
			}
			return Tail.Read(value, source);
		}

		public override void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteFieldHeader(fieldNumber, wireType, dest);
			Tail.Write(value, dest);
		}
	}
}
