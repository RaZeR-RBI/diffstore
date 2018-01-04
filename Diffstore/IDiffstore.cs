using System;
using System.Collections.Generic;
using Diffstore.Entities;
using Diffstore.Snapshots;

namespace Diffstore
{
    /// <summary>
    /// Defines all supported operations.
    /// </summary>
    public interface IDiffstore<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {
        /* Entity-related */

        /// <summary>
        /// Returns an entity saved with the specified key.
        /// </summary>
        Entity<TKey, TValue> this[TKey key] { get; }

        /// <summary>
        /// Returns an entity saved with the specified key.
        /// </summary>
        Entity<TKey, TValue> Get(TKey key);

        /// <summary>
        /// Fetches all existing keys.
        /// </summary>
        IEnumerable<TKey> Keys { get; }

        /// <summary>
        /// Fetches all saved entities.
        /// </summary>
        IEnumerable<Entity<TKey, TValue>> Entities { get; }

        /// <summary>
        /// Saves the entity.
        /// </summary>
        void Save(TKey key, TValue value, bool makeSnapshot = true);

        /// <summary>
        /// Saves the entity.
        /// </summary>
        void Save(Entity<TKey, TValue> entity, bool makeSnapshot = true);

        /// <summary>
        /// Returns true if an entity with the specified key exists.
        /// </summary>
        bool Exists(TKey key);

        /// <summary>
        /// Deletes the entity and all associated data by key.
        /// </summary>
        void Delete(TKey key);

        /// <summary>
        /// Deletes the entity and all associated data by key.
        /// </summary>
        void Delete(Entity<TKey, TValue> entity);

        /// <summary>
        /// Fires when an entity is successfully saved.
        /// </summary>
        EventHandler<Entity<TKey, TValue>> OnSave { get; set; }

        /// <summary>
        /// Fires when an entity with the specified key is deleted.
        /// </summary>
        EventHandler<TKey> OnDelete { get; set; }

        /* Snapshot-related */

        /// <summary>
        /// Fetches snapshots for the specified entity key.
        /// </summary>
        IEnumerable<Snapshot<TKey, TValue>> GetSnapshots(TKey key);

        /// <summary>
        /// Fetches snapshots page for the specified entity key.
        /// </summary>
        IEnumerable<Snapshot<TKey, TValue>> GetSnapshots(TKey key, int from, int count);

        /// <summary>
        /// Fetches snapshots in time range [timeStart, timeEnd).
        /// </summary>
        IEnumerable<Snapshot<TKey, TValue>> GetSnapshotsBetween(TKey key, long timeStart, long timeEnd);

        /// <summary>
        /// Saves an entity snapshot with the specified time.
        /// </summary>
        void PutSnapshot(Entity<TKey, TValue> entity, long time);

        /// <summary>
        /// Returns the time when the first entity snapshot was created.
        /// </summary>
        long GetFirstTime(TKey key);

        /// <summary>
        /// Returns the time when the last entity snapshot was created.
        /// </summary>
        long GetLastTime(TKey key);

        /// <summary>
        /// Returns the first snapshot of the specified entity.
        /// </summary>
        Snapshot<TKey, TValue> GetFirst(TKey key);

        /// <summary>
        /// Returns the last snapshot of the specified entity.
        /// </summary>
        Snapshot<TKey, TValue> GetLast(TKey key);
    }
}