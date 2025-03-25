using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
	public sealed class SubType
	{
		internal sealed class Comparer : IComparer<SubType>, IComparer
		{
			public static readonly Comparer Default = new Comparer();

			public int Compare(object x, object y)
			{
				return Compare(x as SubType, y as SubType);
			}

			public int Compare(SubType x, SubType y)
			{
				if (object.ReferenceEquals(x, y))
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				return x.FieldNumber.CompareTo(y.FieldNumber);
			}
		}

		private readonly int fieldNumber;

		private readonly MetaType derivedType;

		private readonly DataFormat dataFormat;

		private IProtoSerializer serializer;

		public int FieldNumber
		{
			get
			{
				return fieldNumber;
			}
		}

		public MetaType DerivedType
		{
			get
			{
				return derivedType;
			}
		}

		internal IProtoSerializer Serializer
		{
			get
			{
				if (serializer == null)
				{
					serializer = BuildSerializer();
				}
				return serializer;
			}
		}

		public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
		{
			if (derivedType == null)
			{
				throw new ArgumentNullException("derivedType");
			}
			if (fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			this.fieldNumber = fieldNumber;
			this.derivedType = derivedType;
			dataFormat = format;
		}

		private IProtoSerializer BuildSerializer()
		{
			WireType wireType = WireType.String;
			if (dataFormat == DataFormat.Group)
			{
				wireType = WireType.StartGroup;
			}
			IProtoSerializer tail = new SubItemSerializer(derivedType.Type, derivedType.GetKey(false, false), derivedType, false);
			return new TagDecorator(fieldNumber, wireType, false, tail);
		}
	}
}
