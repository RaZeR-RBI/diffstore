using System;
using Diffstore.Entities;

/// <summary>
/// Defines snapshot-related functionality.
/// </summary>
namespace Diffstore.Snapshots
{
    /// <summary>
    /// Defines a snapshot of an entity.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type used as entity key.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The entity value, a class which contains the data that need to be stored.
    /// </typeparam>
    /// <seealso cref="DiffstoreBuilder<TKey, TValue>"/>
    /// <seealso cref="IDiffstore<TKey, TValue>"/>
    public class Snapshot<TKey, TValue>
    {
        /// <summary>
        /// Unix time when the snapshot was created
        /// </summary>
        public long Time { get; }

        /// <summary>
        /// Entity value at the moment of saving.
        /// Fields and properties marked by <see cref="IgnoreChangesAttribute"/>
        /// will be initialized with their default values.
        /// </summary>
        public Entity<TKey, TValue> State { get; }

        /// <summary>
        /// Creates a snapshot of a given entity marked with specified time.
        /// </summary>
        /// <param name="time">Snapshot creation time.</param>
        /// <param name="state">Entity value at the specified moment.</param>
        public Snapshot(long time, Entity<TKey, TValue> state) =>
            (Time, State) = (time, state);

        /// <summary>
        /// Checks for snapshot equality by time and state.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as Snapshot<TKey, TValue>;
            if ((System.Object)p == null) return false;

            return (Time.Equals(p.Time)) && (State.Equals(p.State));
        }

        /// <summary>
        /// Returns object's hashcode
        /// </summary>
        public override int GetHashCode() =>
            unchecked(Time.GetHashCode() + State.GetHashCode());

        /// <summary>
        /// Returns a string representation
        /// </summary>
        public override string ToString() =>
            $"T: {Time}, S: {State.ToString()}";
    }

    /// <summary>
    /// Static class with snapshot-related helper functions.
    /// </summary>
    public static class Snapshot
    {
        /// <summary>
        /// Creates a snapshot of a given entity marked with specified time.
        /// </summary>
        /// <param name="time">Snapshot creation time.</param>
        /// <param name="state">Entity value at the specified moment.</param>
        public static Snapshot<TK, TV> Create<TK, TV>(long time, Entity<TK, TV> entity) =>
            new Snapshot<TK, TV>(time, entity);

        /// <summary>
        /// Creates a snapshot of a given entity marked with current time.
        /// </summary>
        /// <param name="state">Entity value at the specified moment.</param>
        public static Snapshot<TK, TV> Create<TK, TV>(Entity<TK, TV> entity) =>
            Create(GetCurrentUnixSeconds(), entity);

        /// <summary>
        /// Returns current time in unix seconds.
        /// </summary>
        public static long GetCurrentUnixSeconds() =>
            ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
    }
}