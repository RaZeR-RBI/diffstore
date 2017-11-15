using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Diffstore.Serialization.File;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem;
using SharpFileSystem.FileSystems;

namespace Diffstore
{
    public enum FileFormat
    {
        XML, JSON, Binary
    }

    // TODO: Rewrite the builder to allow calls in any order
    public class DiffstoreBuilder<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
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

        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities(
            FileFormat format,
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
        {
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);

            switch (format)
            {
                case FileFormat.Binary:
                    return WithFileBasedEntities(options, fileSystem);
                case FileFormat.XML:
                    var formatter = new XmlFileFormatter();
                    var entityIO = new FilesystemEntityReaderWriter
                        <TKey, XmlDocumentAdapter, XmlWriterAdapter>
                        (fileSystem, formatter, options);
                    em = new EntityManager<TKey, TValue, XmlDocumentAdapter, XmlWriterAdapter>
                        (formatter, entityIO);
                    return this;
                default:
                    throw new NotImplementedException();
            }
        }

        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities<TIn, TOut>(
            IFormatter<TIn, TOut> formatter,
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
            where TIn : IDisposable
            where TOut : IDisposable
        {
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);
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
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);
            sm = new BinaryLIFOSnapshotManager<TKey, TValue>(options,
                fileSystem, zeroPaddingBytes);
            return this;
        }

        public DiffstoreBuilder<TKey, TValue> WithSingleFileSnapshots(
            FileFormat format,
            IFileSystem fileSystem = null,
            FilesystemStorageOptions options = null)
        {
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);

            switch(format) {
                case FileFormat.XML: 
                    sm = new SingleFileSnapshotManager<TKey, TValue, XmlDocumentAdapter, XmlWriterAdapter>
                    (options, new XmlFileFormatter(), fileSystem); break;
                default:
                    throw new NotImplementedException();
            }

            return this;
        }

        public IDiffstore<TKey, TValue> Setup()
        {
            if (em == null || sm == null) throw new ArgumentNullException(
                "Entity and snapshot storage must be configured first"
            );
            return new DS<TKey, TValue>(em, sm);
        }

        private (IFileSystem, FilesystemStorageOptions) PrepareFileStorage(
            IFileSystem fileSystem, FilesystemStorageOptions options)
        {
            if (options == null) options = new FilesystemStorageOptions();
            if (fileSystem == null) fileSystem = this.fileSystem;

            if (fileSystem == null) throw new InvalidOperationException(
                "To use file-based snapshots, select the filesystem type " +
                "by calling either InMemory() or OnDisk(...)"
            );
            return (fileSystem, options);
        }
    }
}