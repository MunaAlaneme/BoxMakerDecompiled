using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class UriDecorator : ProtoDecoratorBase
	{
		private static readonly Type expectedType = typeof(Uri);

		public override Type ExpectedType
		{
			get
			{
				return expectedType;
			}
		}

		public override bool RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		public override bool ReturnsValue
		{
			get
			{
				return true;
			}
		}

		public UriDecorator(TypeModel model, IProtoSerializer tail)
			: base(tail)
		{
		}

		public override void Write(object value, ProtoWriter dest)
		{
			Tail.Write(((Uri)value).AbsoluteUri, dest);
		}

		public override object Read(object value, ProtoReader source)
		{
			string text = (string)Tail.Read(null, source);
			return (text.Length != 0) ? new Uri(text) : null;
		}
	}
}
