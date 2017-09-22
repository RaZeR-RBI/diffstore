using System;

namespace Diffstore.Entities
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DoNotPersistAttribute : Attribute
    {
        public DoNotPersistAttribute () { }
    }
}