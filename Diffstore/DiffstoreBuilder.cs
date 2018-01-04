using System;
using System.IO;
using System.Reflection;
using System.Xml;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Diffstore.Serialization.JSON;
using Diffstore.Serialization.XML;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem;
using SharpFileSystem.FileSystems;

namespace Diffstore
{

    /// <summary>
    /// Defines the format for file-based storage.
    /// </summary>
    public enum FileFormat
    {
        XML, JSON, Binary
    }

    // TODO: Rewrite the builder to allow calls in any order
    /// <summary>
    /// The main starting point, used to create a <see cref="IDiffstore<TKey, TValue>"/> instance.
    /// </summary>
    /// <typeparam name="TKey">
    /// The type used as entity key.
    /// All numeric types and strings are supported.
    /// Any IComparable with a corresponding string representation should work.
    /// </typeparam>
    /// <typeparam name="TValue">
    /// The entity value, a class which contains the data that need to be stored.
    /// Must be a class with a parameterless constructor.
    /// </typeparam>
    /// <example>
    /// To create a new instance, first, choose a storage option - it's either
    /// <see cref="WithMemoryStorage()"/> or <see cref="WithDiskStorage(string)"/>:
    /// <code>
    /// var db = new DiffstoreBuilder<long, MyData>()
    ///                 .WithDiskStorage();
    /// </code>
    /// <para>
    /// Then you should use the type of storage for entities and snapshots.
    /// At this time all entities are file-based, but in the future they may be stored
    /// in relational databases or something other. Available formats are defined in
    /// <see cref="FileFormat"/>.
    /// </para>
    /// <para>
    /// There are several mechanisms snapshot storage - currently implemented are
    /// "single file per snapshot" (see <see cref="FileFormat"/>) and 
    /// "last in, first out" (optimized binary storage).
    /// </para>
    /// <para>
    /// For example, to store your entities and snapshots in JSON files:
    /// </para>
    /// <code>
    /// var db = new DiffstoreBuilder<long, MyData>()
    ///                 .WithDiskStorage()
    ///                 .WithFileBasedEntities(FileFormat.JSON)
    ///                 .WithSingleFileSnapshots(FileFormat.JSON);
    ///                 .Setup();
    /// </code>
    /// And that's it! Now you can use the storage as you like. Check out the
    /// <see cref="IDiffstore<TKey, TValue>"/> interface to see what can you do.
    /// </example>
    /// <remarks>
    /// This API may change in the near future to allow function calls in any order.
    /// </remarks>
    /// <seealso cref="IDiffstore<TKey, TValue>"/>
    /// <seealso cref="FileFormat"/>
    public class DiffstoreBuilder<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {
        internal IEntityManager<TKey, TValue> em;
        internal ISnapshotManager<TKey, TValue> sm;
        protected IFileSystem fileSystem;

        /// <summary>
        /// Sets up the engine to use in-memory storage.
        /// </summary>
        public DiffstoreBuilder<TKey, TValue> WithMemoryStorage()
        {
            fileSystem = new MemoryFileSystem();
            return this;
        }

        /// <summary>
        /// Sets up the engine to use disk storage.
        /// </summary>
        /// <param name="subfolder">
        /// Subfolder for files relative to the app location.
        /// Defaults to "storage".
        /// </param>
        public DiffstoreBuilder<TKey, TValue> WithDiskStorage(string subfolder = "storage")
        {
            var appRoot = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var storagePath = Path.Combine(appRoot, subfolder);
            fileSystem = new PhysicalFileSystem(storagePath);
            if (!System.IO.Directory.Exists(storagePath))
                System.IO.Directory.CreateDirectory(storagePath);
            return this;
        }

        /// <summary>
        /// Sets up the engine to use file-based entity storage.
        /// </summary>
        /// <param name="options">
        /// If not null, overrides the default <see cref="FilesystemStorageOptions"/>.
        /// </param>
        /// <param name="fileSystem">
        /// If not null, uses the specified IFileSystem, otherwise uses
        /// what has been called before.
        /// </param>
        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities(
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
        {
            return WithFileBasedEntities(FastBinaryFormatter.Instance, options, fileSystem);
        }

        /// <summary>
        /// Sets up the engine to use file-based entity storage.
        /// </summary>
        /// <param name="format">
        /// Specifies the <see cref="FileFormat"/> which will be used for
        /// serialization and deserialization.
        /// </param>
        /// <param name="options">
        /// If not null, overrides the default <see cref="FilesystemStorageOptions"/>.
        /// </param>
        /// <param name="fileSystem">
        /// If not null, uses the specified IFileSystem, otherwise uses
        /// what has been called before.
        /// </param>
        public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities(
            FileFormat format,
            FilesystemStorageOptions options = null,
            IFileSystem fileSystem = null)
        {
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);

            switch (format)
            {
                case FileFormat.Binary:
                    return WithFileBasedEntities(FastBinaryFormatter.Instance, options, fileSystem);
                case FileFormat.XML:
                    return WithFileBasedEntities(XmlFormatter.Instance, options, fileSystem);
                case FileFormat.JSON:
                    return WithFileBasedEntities(JsonFormatter.Instance, options, fileSystem);
                default:
                    throw new InvalidOperationException("Unsupported format");
            }
        }

        /// <summary>
        /// Sets up the engine to use file-based entity storage.
        /// </summary>
        /// <param name="formatter">
        /// Specifies a <see cref="IFormatter<TInputStream, TOutputStream>"/> which
        /// will be used for serialization and deserialization.
        /// </param>
        /// <param name="options">
        /// If not null, overrides the default <see cref="FilesystemStorageOptions"/>.
        /// </param>
        /// <param name="fileSystem">
        /// If not null, uses the specified IFileSystem, otherwise uses
        /// what has been called before.
        /// </param>
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

        /// <summary>
        /// Sets up the engine to use the last-in-first-out optimized snapshot storage,
        /// suitable for fast reading of recent data.
        /// </summary>
        /// <param name="formatter">
        /// Specifies a <see cref="IFormatter<TInputStream, TOutputStream>"/> which
        /// will be used for serialization and deserialization.
        /// </param>
        /// <param name="options">
        /// If not null, overrides the default <see cref="FilesystemStorageOptions"/>.
        /// </param>
        /// <param name="zeroPaddingBytes">
        /// Specifies maximum free space at the start of the file for future
        /// snapshots. When there is no free space left for writing, the file
        /// expands by specified byte count and rewrites.
        /// <para>
        /// If the number is too small, rewrites may appear often.
        /// </para>
        /// <para>
        /// If the number is big, files will be bigger and may consist
        /// of big unused space depending on how many data is being written.
        /// </para>
        /// </param>
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

        /// <summary>
        /// Sets up the engine to use the single-file-per-snapshot storage.
        /// Suitable for big entities and/or when they need to be human-readable.
        /// </summary>
        /// <param name="format">
        /// Specifies the <see cref="FileFormat"/> which will be used for
        /// serialization and deserialization.
        /// </param>
        /// <param name="options">
        /// If not null, overrides the default <see cref="FilesystemStorageOptions"/>.
        /// </param>
        /// <param name="fileSystem">
        /// If not null, uses the specified IFileSystem, otherwise uses
        /// what has been called before.
        /// </param>
        public DiffstoreBuilder<TKey, TValue> WithSingleFileSnapshots(
            FileFormat format,
            IFileSystem fileSystem = null,
            FilesystemStorageOptions options = null)
        {
            (fileSystem, options) = PrepareFileStorage(fileSystem, options);

            switch (format)
            {
                case FileFormat.XML:
                    sm = new SingleFileSnapshotManager<TKey, TValue, XmlDocumentAdapter, XmlWriterAdapter>
                    (options, new XmlFormatter(), fileSystem); break;
                case FileFormat.JSON:
                    sm = new SingleFileSnapshotManager<TKey, TValue, JsonReaderAdapter, JsonWriterAdapter>
                    (options, JsonFormatter.Instance, fileSystem); break;
                default:
                    sm = new SingleFileSnapshotManager<TKey, TValue, BinaryReader, BinaryWriter>
                    (options, FastBinaryFormatter.Instance, fileSystem); break;
            }

            return this;
        }

        /// <summary>
        /// Builds and returns the instance.
        /// </summary>
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