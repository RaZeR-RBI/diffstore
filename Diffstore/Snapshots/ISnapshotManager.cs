using System;
using System.Collections.Generic;
using Diffstore.Entities;
using Diffstore.Snapshots;

namespace Diffstore.Snapshots
{
    public interface ISnapshotManager<TKey, TValue> : IDisposable
        where TKey : IComparable
        where TValue : class, new()
    {
        bool Any(TKey key);
        long GetFirstTime(TKey key);
        long GetLastTime(TKey key);
        Snapshot<TKey, TValue> GetFirst(TKey key);
        Snapshot<TKey, TValue> GetLast(TKey key);
        IEnumerable<Snapshot<TKey, TValue>> GetAll(TKey key);
        IEnumerable<Snapshot<TKey, TValue>> GetPage(TKey key, int from, int count, 
            bool ascending = false);
        IEnumerable<Snapshot<TKey, TValue>> GetInRange(TKey key, long timeStart, long timeEnd);
        void Make(Entity<TKey, TValue> entity);
        void Make(Entity<TKey, TValue> entity, long time);
        void Drop(TKey entityKey);
    }
}