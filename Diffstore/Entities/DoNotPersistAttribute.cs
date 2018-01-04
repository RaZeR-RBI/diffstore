using System;

namespace Diffstore.Entities
{
    /// <summary>
    /// Field or property marked by this attribute will not be saved neither in
    /// entities, neither in snapshots.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DoNotPersistAttribute : Attribute
    {
        /// <summary>
        /// Marks the field or property as non-persistable.
        /// </summary>
        public DoNotPersistAttribute () { }
    }
}