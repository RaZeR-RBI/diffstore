using System;
using System.Collections.Generic;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using SharpFileSystem;

namespace Diffstore.Snapshots.Filesystem
{
    public class IncrementalBinarySnapshotManager<TKey, TValue> : ISnapshotManager<TKey, TValue>
        where TKey : IComparable
        where TValue : new()
    {

        private FilesystemStorageOptions options;
        private IFileSystem fileSystem;
        
        public IncrementalBinarySnapshotManager(FilesystemStorageOptions opts, 
            IFileSystem fs) => (options, fileSystem) = (opts, fs);

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Drop(TKey entityKey)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetAll(TKey key)
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetFirst(TKey key)
        {
            throw new NotImplementedException();
        }

        public long GetFirstTime(TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetInRange(TKey key, long timeStart, long timeEnd)
        {
            throw new NotImplementedException();
        }

        public Snapshot<TKey, TValue> GetLast(TKey key)
        {
            throw new NotImplementedException();
        }

        public long GetLastTime(TKey key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetPage(TKey key, int from, int count)
        {
            throw new NotImplementedException();
        }

        public bool Make(Entity<TKey, TValue> entity, bool onlyIfChanged = true)
        {
            throw new NotImplementedException();
        }

        public bool Make(Entity<TKey, TValue> entity, long time, bool onlyIfChanged = true)
        {
            throw new NotImplementedException();
        }
    }
}
