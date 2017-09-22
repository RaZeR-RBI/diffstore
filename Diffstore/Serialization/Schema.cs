using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Diffstore.Entities;
using Diffstore.Snapshots;
using Fasterflect;

namespace Diffstore.Serialization
{
    public class Schema
    {
        public ReadOnlyCollection<Field> Fields { get; }

        public Schema(Type type, BindingFlags flags = BindingFlags.Public | BindingFlags.Instance)
        {
            var fields = type.GetFields(flags)
                .Where(field => IsPersistable(field))
                .OrderBy(field => field.MetadataToken)
                .Select(field => new Field(field))
                .ToList();

            fields.AddRange(type.GetProperties(flags)
                .Where(prop => IsPersistable(prop))
                .OrderBy(prop => prop.MetadataToken)
                .Select(prop => new Field(prop)));

            Fields = new ReadOnlyCollection<Field>(fields);
        }

        private static bool IsPersistable(FieldInfo field) => 
            !field.GetCustomAttributes(typeof(DoNotPersistAttribute)).Any();

        private static bool IsPersistable(PropertyInfo prop) =>
            !prop.GetCustomAttributes(typeof(DoNotPersistAttribute)).Any();

        public class Field
        {
            public string Name { get; }
            public Type Type { get; }
            public MemberGetter Getter { get; }
            public MemberSetter Setter { get; }

            public bool IgnoreChanges { get; }

            public Field(FieldInfo fieldInfo)
            {
                Name = fieldInfo.Name;
                Type = fieldInfo.FieldType;
                Getter = fieldInfo.DelegateForGetFieldValue();
                Setter = fieldInfo.DelegateForSetFieldValue();
                IgnoreChanges = fieldInfo.GetCustomAttributes(typeof(IgnoreChangesAttribute), true).Any();
            }

            public Field(PropertyInfo propertyInfo)
            {
                Name = propertyInfo.Name;
                Type = propertyInfo.PropertyType;
                Getter = propertyInfo.DelegateForGetPropertyValue();
                Setter = propertyInfo.DelegateForSetPropertyValue();
                IgnoreChanges = propertyInfo.GetCustomAttributes(typeof(IgnoreChangesAttribute), true).Any();
            }

            public void Write(object target, object value) => Setter(target, value);

            public object Read(object target) => Getter(target);
        }
    }
}