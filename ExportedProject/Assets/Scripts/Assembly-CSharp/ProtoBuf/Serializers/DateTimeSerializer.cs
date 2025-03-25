using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class DateTimeSerializer : IProtoSerializer
	{
		private static readonly Type expectedType = typeof(DateTime);

		private readonly bool includeKind;

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

		public DateTimeSerializer(TypeModel model)
		{
			includeKind = model != null && model.SerializeDateTimeKind();
		}

		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadDateTime(source);
		}

		public void Write(object value, ProtoWriter dest)
		{
			if (includeKind)
			{
				BclHelpers.WriteDateTimeWithKind((DateTime)value, dest);
			}
			else
			{
				BclHelpers.WriteDateTime((DateTime)value, dest);
			}
		}
	}
}
