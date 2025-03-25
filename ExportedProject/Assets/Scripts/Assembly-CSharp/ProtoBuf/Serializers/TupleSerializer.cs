using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	internal sealed class TupleSerializer : IProtoSerializer, IProtoTypeSerializer
	{
		private readonly MemberInfo[] members;

		private readonly ConstructorInfo ctor;

		private IProtoSerializer[] tails;

		public Type ExpectedType
		{
			get
			{
				return ctor.DeclaringType;
			}
		}

		public bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		public bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		public TupleSerializer(RuntimeTypeModel model, ConstructorInfo ctor, MemberInfo[] members)
		{
			if (ctor == null)
			{
				throw new ArgumentNullException("ctor");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			this.ctor = ctor;
			this.members = members;
			tails = new IProtoSerializer[members.Length];
			ParameterInfo[] parameters = ctor.GetParameters();
			for (int i = 0; i < members.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				Type itemType = null;
				Type defaultType = null;
				MetaType.ResolveListTypes(model, parameterType, ref itemType, ref defaultType);
				Type type = ((itemType != null) ? itemType : parameterType);
				bool asReference = false;
				int num = model.FindOrAddAuto(type, false, true, false);
				if (num >= 0)
				{
					asReference = model[type].AsReferenceDefault;
				}
				WireType defaultWireType;
				IProtoSerializer protoSerializer = ValueMember.TryGetCoreSerializer(model, DataFormat.Default, type, out defaultWireType, asReference, false, false, true);
				if (protoSerializer == null)
				{
					throw new InvalidOperationException("No serializer defined for type: " + type.FullName);
				}
				protoSerializer = new TagDecorator(i + 1, defaultWireType, false, protoSerializer);
				IProtoSerializer protoSerializer2 = ((itemType != null) ? ((!parameterType.IsArray) ? ((ProtoDecoratorBase)ListDecorator.Create(model, parameterType, defaultType, protoSerializer, i + 1, false, defaultWireType, true, false, false)) : ((ProtoDecoratorBase)new ArrayDecorator(model, protoSerializer, i + 1, false, defaultWireType, parameterType, false, false))) : protoSerializer);
				tails[i] = protoSerializer2;
			}
		}

		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		object IProtoTypeSerializer.CreateInstance(ProtoReader source)
		{
			throw new NotSupportedException();
		}

		bool IProtoTypeSerializer.CanCreateInstance()
		{
			return false;
		}

		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		private object GetValue(object obj, int index)
		{
			PropertyInfo propertyInfo;
			if ((propertyInfo = members[index] as PropertyInfo) != null)
			{
				if (obj == null)
				{
					return (!Helpers.IsValueType(propertyInfo.PropertyType)) ? null : Activator.CreateInstance(propertyInfo.PropertyType);
				}
				return propertyInfo.GetValue(obj, null);
			}
			FieldInfo fieldInfo;
			if ((fieldInfo = members[index] as FieldInfo) != null)
			{
				if (obj == null)
				{
					return (!Helpers.IsValueType(fieldInfo.FieldType)) ? null : Activator.CreateInstance(fieldInfo.FieldType);
				}
				return fieldInfo.GetValue(obj);
			}
			throw new InvalidOperationException();
		}

		public object Read(object value, ProtoReader source)
		{
			object[] array = new object[members.Length];
			bool flag = false;
			if (value == null)
			{
				flag = true;
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = GetValue(value, i);
			}
			int num;
			while ((num = source.ReadFieldHeader()) > 0)
			{
				flag = true;
				if (num <= tails.Length)
				{
					IProtoSerializer protoSerializer = tails[num - 1];
					array[num - 1] = tails[num - 1].Read((!protoSerializer.RequiresOldValue) ? null : array[num - 1], source);
				}
				else
				{
					source.SkipField();
				}
			}
			return (!flag) ? value : ctor.Invoke(array);
		}

		public void Write(object value, ProtoWriter dest)
		{
			for (int i = 0; i < tails.Length; i++)
			{
				object value2 = GetValue(value, i);
				if (value2 != null)
				{
					tails[i].Write(value2, dest);
				}
			}
		}

		private Type GetMemberType(int index)
		{
			Type memberType = Helpers.GetMemberType(members[index]);
			if (memberType == null)
			{
				throw new InvalidOperationException();
			}
			return memberType;
		}
	}
}
