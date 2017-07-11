using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Diffstore.Serialization
{
    public class Schema
    {
        public ReadOnlyCollection<Field> Fields { get; }

        public Schema(Type type, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var fields = type.GetFields(flags)
                .OrderBy(field => field.MetadataToken)
                .Select(field => new Field(field))
                .ToList();

            fields.AddRange(type.GetProperties(flags)
                .OrderBy(prop => prop.MetadataToken)
                .Select(prop => new Field(prop)));

            Fields = new ReadOnlyCollection<Field>(fields);
        }


        public class Field
        {
            public string Name { get; }
            public Type Type { get; }
            public MemberGetter Getter { get; }
            public MemberSetter Setter { get; }

            public Field(FieldInfo fieldInfo)
            {
                Name = fieldInfo.Name;
                Type = fieldInfo.FieldType;
                Getter = fieldInfo.DelegateForGetFieldValue();
                Setter = fieldInfo.DelegateForSetFieldValue();
            }

            public Field(PropertyInfo propertyInfo)
            {
                Name = propertyInfo.Name;
                Type = propertyInfo.PropertyType;
                Getter = propertyInfo.DelegateForGetPropertyValue();
                Setter = propertyInfo.DelegateForSetPropertyValue();
            }

            public void Write(object target, object value)
            {
                Setter(target, value);
            }

            public object Read(object target)
            {
                return Getter(target);
            }
        }
    }
}