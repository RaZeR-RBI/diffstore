using System;
using System.Collections.Generic;
using Diffstore.Entities;
using Diffstore.Snapshots;

namespace Diffstore.Snapshots
{
    public interface ISnapshotManager<TKey, TValue> : IDisposable
        where TKey : IComparable
        where TValue : new()
    {
        long GetFirstTime(TKey key);
        long GetLastTime(TKey key);
        Snapshot<TKey, TValue> GetFirst(TKey key);
        Snapshot<TKey, TValue> GetLast(TKey key);
        IEnumerable<Snapshot<TKey, TValue>> GetAll(TKey key);
        IEnumerable<Snapshot<TKey, TValue>> GetPage(TKey key, int from, int count);
        IEnumerable<Snapshot<TKey, TValue>> GetInRange(TKey key, long timeStart, long timeEnd);
        bool Make(Entity<TKey, TValue> entity, bool onlyIfChanged = true);
        bool Make(Entity<TKey, TValue> entity, long time, bool onlyIfChanged = true);
        void Drop(TKey entityKey);
    }
}