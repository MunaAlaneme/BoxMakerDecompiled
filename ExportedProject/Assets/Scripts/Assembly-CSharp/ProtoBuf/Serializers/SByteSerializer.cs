using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class SByteSerializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(sbyte);

		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		public Type ExpectedType
		{
			get
			{
				return expectedType;
			}
		}

		public SByteSerializer(TypeModel model)
		{
		}

		public object Read(object value, ProtoReader source)
		{
			return source.ReadSByte();
		}

		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteSByte((sbyte)value, dest);
		}
	}
}
