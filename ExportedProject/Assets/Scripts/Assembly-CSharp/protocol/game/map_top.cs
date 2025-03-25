using System;
using System.ComponentModel;
using ProtoBuf;

namespace protocol.game
{
	[Serializable]
	[ProtoContract(Name = "map_top")]
	public class map_top : IExtensible
	{
		private int _id;

		private string _name = string.Empty;

		private byte[] _url;

		private int _rank;

		private IExtension extensionObject;

		[DefaultValue(0)]
		[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
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

		[DefaultValue("")]
		[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

		[DefaultValue(null)]
		[ProtoMember(3, IsRequired = false, Name = "url", DataFormat = DataFormat.Default)]
		public byte[] url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
			}
		}

		[DefaultValue(0)]
		[ProtoMember(4, IsRequired = false, Name = "rank", DataFormat = DataFormat.TwosComplement)]
		public int rank
		{
			get
			{
				return _rank;
			}
			set
			{
				_rank = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
