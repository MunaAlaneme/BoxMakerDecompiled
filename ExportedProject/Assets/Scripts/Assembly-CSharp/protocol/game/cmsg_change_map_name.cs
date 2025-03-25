using System;
using ProtoBuf;

namespace protocol.game
{
	[Serializable]
	[ProtoContract(Name = "cmsg_change_map_name")]
	public class cmsg_change_map_name : IExtensible
	{
		private msg_common _common;

		private int _id;

		private string _name;

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

		[ProtoMember(2, IsRequired = true, Name = "id", DataFormat = DataFormat.TwosComplement)]
		public int id
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}

		[ProtoMember(3, IsRequired = true, Name = "name", DataFormat = DataFormat.Default)]
		public string name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
