using System;
using System.ComponentModel;
using ProtoBuf;

namespace protocol.game
{
	[Serializable]
	[ProtoContract(Name = "comment")]
	public class comment : IExtensible
	{
		private int _head;

		private string _name = string.Empty;

		private string _country = string.Empty;

		private string _text = string.Empty;

		private string _date = string.Empty;

		private int _userid;

		private int _visitor;

		private IExtension extensionObject;

		[DefaultValue(0)]
		[ProtoMember(1, IsRequired = false, Name = "head", DataFormat = DataFormat.TwosComplement)]
		public int head
		{
			get
			{
				return _head;
			}
			set
			{
				_head = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
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

		[DefaultValue("")]
		[ProtoMember(3, IsRequired = false, Name = "country", DataFormat = DataFormat.Default)]
		public string country
		{
			get
			{
				return _country;
			}
			set
			{
				_country = value;
			}
		}

		[DefaultValue("")]
		[ProtoMember(4, IsRequired = false, Name = "text", DataFormat = DataFormat.Default)]
		public string text
		{
			get
			{
				return _text;
			}
			set
			{
				_text = value;
			}
		}

		[DefaultValue("")]
		[ProtoMember(5, IsRequired = false, Name = "date", DataFormat = DataFormat.Default)]
		public string date
		{
			get
			{
				return _date;
			}
			set
			{
				_date = value;
			}
		}

		[DefaultValue(0)]
		[ProtoMember(6, IsRequired = false, Name = "userid", DataFormat = DataFormat.TwosComplement)]
		public int userid
		{
			get
			{
				return _userid;
			}
			set
			{
				_userid = value;
			}
		}

		[DefaultValue(0)]
		[ProtoMember(7, IsRequired = false, Name = "visitor", DataFormat = DataFormat.TwosComplement)]
		public int visitor
		{
			get
			{
				return _visitor;
			}
			set
			{
				_visitor = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
