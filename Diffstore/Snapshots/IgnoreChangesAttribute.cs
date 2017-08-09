using System;

namespace Diffstore.Snapshots
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IgnoreChangesAttribute : Attribute
    {
        public IgnoreChangesAttribute () { }
    }
}