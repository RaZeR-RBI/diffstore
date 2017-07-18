using System;

namespace Diffstore.Snapshots.Filesystem
{
    public class BinarySnapshotManager<TKey, TValue> : ISnapshotManager<TKey, TValue>
        where TKey : IComparable
        where TValue : new()
    {
        public void Dispose() { }

        public void Drop(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetAll()
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetInRange(long timeStart, long timeEnd)
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetLast()
        {
            throw new NotImplementedException();
        }

        public long GetLastChangeTime()
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetPage(int from, int count)
        {
            throw new NotImplementedException();
        }
    }
}