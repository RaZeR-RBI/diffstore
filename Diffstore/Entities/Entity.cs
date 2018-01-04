using System;
using Diffstore.Serialization;

/// <summary>
/// Defines entity-related functionality.
/// </summary>
namespace Diffstore.Entities
{
    /// <summary>
    /// Defines an entity.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type used as entity key.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The entity value, a class which contains the data that need to be stored.
    /// </typeparam>
    /// <seealso cref="DiffstoreBuilder<TKey, TValue>"/>
    /// <seealso cref="IDiffstore<TKey, TValue>"/>
    public class Entity<TKey, TValue>
    {
        /// <summary>
        /// The entity's key.
        /// </summary>
        /// <returns></returns>
        public TKey Key { get; set; }
        /// <summary>
        /// The entity's value.
        /// </summary>
        /// <returns></returns>
        public TValue Value { get; set; }

        /// <summary>
        /// Creates an entity with the corresponding key and value.
        /// </summary>
        public Entity(TKey key, TValue value) => (Key, Value) = (key, value);

        /// <summary>
        /// Checks equality agains other objects.
        /// If the supplied object is an entity of the same type,
        /// compares the keys and values.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as Entity<TKey, TValue>;
            if ((System.Object)p == null) return false;

            return (Key.Equals(p.Key)) && (Value.Equals(p.Value));
        }

        /// <summary>
        /// Calculates hashcode based on key and value hashcodes.
        /// </summary>
        public override int GetHashCode() =>
            unchecked(Key.GetHashCode() + Value.GetHashCode());

        /// <summary>
        /// Returns a human-readable string representations.
        /// </summary>
        public override string ToString() => $"[{Key} => {Value}]";
    }

    /// <summary>
    /// Defines an entity-related static class with helper functions.
    /// </summary>
    public static class Entity
    {
        /// <summary>
        /// Creates an entity with the specified key and value.
        /// </summary>
        public static Entity<TK, TV> Create<TK, TV>(TK key, TV value) =>
            new Entity<TK, TV>(key, value);
    }
}