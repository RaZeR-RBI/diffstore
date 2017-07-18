using System;
using Diffstore.Snapshots;

namespace Diffstore.Snapshots
{
    public interface ISnapshotManager<TKey, TValue> : IDisposable
        where TKey : IComparable
        where TValue : new()
    {
        long GetLastChangeTime();
        Snapshot<TKey, TValue> GetLast();
        Snapshot<TKey, TValue> GetAll();
        Snapshot<TKey, TValue> GetPage(int from, int count);
        Snapshot<TKey, TValue> GetInRange(long timeStart, long timeEnd);
        void Drop(TKey entityKey);
    }
}