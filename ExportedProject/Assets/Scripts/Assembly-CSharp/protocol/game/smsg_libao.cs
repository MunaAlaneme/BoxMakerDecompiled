using System;
using System.ComponentModel;
using ProtoBuf;

namespace protocol.game
{
	[Serializable]
	[ProtoContract(Name = "smsg_libao")]
	public class smsg_libao : IExtensible
	{
		private int _life;

		private IExtension extensionObject;

		[DefaultValue(0)]
		[ProtoMember(1, IsRequired = false, Name = "life", DataFormat = DataFormat.TwosComplement)]
		public int life
		{
			get
			{
				return _life;
			}
			set
			{
				_life = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
