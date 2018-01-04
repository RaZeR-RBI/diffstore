using System;
using System.Collections.Generic;
using Diffstore.Entities;
using Diffstore.Snapshots;

/// <summary>
/// 
/// </summary>
namespace Diffstore
{
    /// <summary>
    /// Default implementation. Use <see cref="DiffstoreBuilder"/> for creation.
    /// </summary>
    internal class DS<TKey, TValue> : IDiffstore<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {
        private readonly IEntityManager<TKey, TValue> em;
        private readonly ISnapshotManager<TKey, TValue> sm;

        public DS(IEntityManager<TKey, TValue> em, 
            ISnapshotManager<TKey, TValue> sm) => (this.em, this.sm) = (em, sm);

        public Entity<TKey, TValue> this[TKey key] => Get(key);

        public IEnumerable<TKey> Keys => em.GetKeys();

        public IEnumerable<Entity<TKey, TValue>> Entities => em.GetAll();

        public EventHandler<Entity<TKey, TValue>> OnSave { get; set; }
        public EventHandler<TKey> OnDelete { get; set; }

        public void Delete(TKey key)
        {
            if (!Exists(key)) return;
            sm.Drop(key);
            em.Delete(key);
            OnDelete?.Invoke(this, key);
        }

        public void Delete(Entity<TKey, TValue> entity) => Delete(entity.Key);

        public bool Exists(TKey key) => em.Exists(key);

        public Entity<TKey, TValue> Get(TKey key) => em.Get(key);

        public Snapshot<TKey, TValue> GetFirst(TKey key) => sm.GetFirst(key);

        public long GetFirstTime(TKey key) => sm.GetFirstTime(key);

        public Snapshot<TKey, TValue> GetLast(TKey key) => sm.GetLast(key);

        public long GetLastTime(TKey key) => sm.GetLastTime(key);

        public IEnumerable<Snapshot<TKey, TValue>> GetSnapshots(TKey key) => 
            sm.GetAll(key);

        public IEnumerable<Snapshot<TKey, TValue>> GetSnapshots(
            TKey key, int from, int count) => sm.GetPage(key, from, count);

        public IEnumerable<Snapshot<TKey, TValue>> GetSnapshotsBetween(TKey key,
            long timeStart, long timeEnd) => sm.GetInRange(key, timeStart, timeEnd);

        public void Save(TKey key, TValue value, bool makeSnapshot) => 
            Save(Entity.Create(key, value), makeSnapshot);

        public void Save(Entity<TKey, TValue> entity, bool makeSnapshot)
        {
            var old = em.Exists(entity.Key) ? em.Get(entity.Key) : null;
            var changed = old == null || !entity.Value.Equals(old.Value);
            if (changed)
            {
                em.Persist(entity);
                OnSave?.Invoke(this, entity);
                if (makeSnapshot) sm.Make(entity);
            }
        }

        public void PutSnapshot(Entity<TKey, TValue> entity, long time) => 
            sm.Make(entity, time);
    }
}