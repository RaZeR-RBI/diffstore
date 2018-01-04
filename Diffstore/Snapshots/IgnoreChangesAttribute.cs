using System;

namespace Diffstore.Snapshots
{
    /// <summary>
    /// Fields and properties marked by this attribute will not be saved
    /// in snapshots, but included in entities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class IgnoreChangesAttribute : Attribute
    {
        /// <summary>
        /// Marks the field or property as ignored in snapshots.
        /// </summary>
        public IgnoreChangesAttribute () { }
    }
}