using System;
using System.IO;
using System.Reflection;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem;
using SharpFileSystem.FileSystems;

namespace Diffstore
{
    public class DiffstoreBuilder<TKey, TValue>
        where TKey : IComparable
        where TValue : new()
    {
        protected IEntityManager<TKey, TValue> em;
        protected ISnapshotManager<TKey, TValue> sm;
        protected IFileSystem fileSystem;

        public DiffstoreBuilder<TKey, TValue> WithMemoryStorage()
        {
            fileSystem = new MemoryFileSystem();
            return this;
        }

        public DiffstoreBuilder<TKey, TValue> WithDiskStorage(string subfolder = "storage")
        {
            var appRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            fileSystem = new PhysicalFileSystem(Path.Combine(appRoot, subfolder));
            return this;
        }

        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities(
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
        {
            return WithFileBasedEntities(FastBinaryFormatter.Instance, options, fileSystem);
        }

        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities<TIn, TOut>(
            IFormatter<TIn, TOut> formatter,
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
            where TIn : IDisposable
            where TOut : IDisposable
        {
            if (options == null) options = new FilesystemStorageOptions();
            if (fileSystem == null) fileSystem = this.fileSystem;

            if (fileSystem == null) throw new InvalidOperationException(
                "To use file-based entities, select the filesystem type " +
                "by calling either InMemory() or OnDisk(...)"
            );

            var entityIO = new FilesystemEntityReaderWriter<TKey, TIn, TOut>(
                fileSystem, formatter, options);
            em = new EntityManager<TKey, TValue, TIn, TOut>(formatter, entityIO);
            return this;
        }

        public DiffstoreBuilder<TKey, TValue> WithLastFirstOptimizedSnapshots(
            IFileSystem fileSystem = null,
            FilesystemStorageOptions options = null,
            int zeroPaddingBytes = 256)
        {
            if (options == null) options = new FilesystemStorageOptions();
            if (fileSystem == null) fileSystem = this.fileSystem;

            if (fileSystem == null) throw new InvalidOperationException(
                "To use file-based snapshots, select the filesystem type " +
                "by calling either InMemory() or OnDisk(...)"
            );

            sm = new IncrementalBinarySnapshotManager<TKey, TValue>(options, 
                fileSystem, zeroPaddingBytes);
            return this;
        }
        
        public IDiffstore<TKey, TValue> Setup()
        {
            if (em == null || sm == null) throw new ArgumentNullException(
                "Entity and snapshot storage must be configured first"
            );
            return new DS<TKey, TValue>(em, sm);
        }
    }
}