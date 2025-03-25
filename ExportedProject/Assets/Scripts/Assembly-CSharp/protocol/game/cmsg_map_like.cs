using System;
using ProtoBuf;

namespace protocol.game
{
	[Serializable]
	[ProtoContract(Name = "cmsg_map_like")]
	public class cmsg_map_like : IExtensible
	{
		private msg_common _common;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "common", DataFormat = DataFormat.Default)]
		public msg_common common
		{
			get
			{
				return _common;
			}
			set
			{
				_common = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
