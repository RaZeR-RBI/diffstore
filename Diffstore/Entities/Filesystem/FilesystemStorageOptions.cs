using System;
using SharpFileSystem;

namespace Diffstore.Entities.Filesystem
{
    /// <summary>
    /// Defines options used by file-based storage mechanisms.
    /// </summary>
    /// <seealso cref="DiffstoreBuilder<TKey, TValue>"/>
    /// <seealso cref="WithFileBasedEntities(FileFormat, FilesystemStorageOptions, IFileSystem)"/>
    /// <seealso cref="WithFileBasedEntities(FilesystemStorageOptions, IFileSystem)"/>
    /// <seealso cref="WithFileBasedEntities<TIn, TOut>(IFormatter<TIn, TOut>, FilesystemStorageOptions, IFileSystem)"/>
    public class FilesystemStorageOptions
    {
        /// <summary>
        /// Gets or sets the root directory for the storage backend.
        /// Defaults to current directory.
        /// </summary>
        public FileSystemPath BasePath { get; set; }

        /// <summary>
        /// Specifies entity per directory count (0 means that all entities 
        /// will be in one directory).
        /// </summary>
        public int EntitiesPerDirectory { get; set; } = 1000;

        /// <summary>
        /// Specifies the maximum snapshot file size. Defaults to 1 MB.
        /// Note: the file may contain one or more snapshots depending on
        /// the used ISnapshotManager implementation.
        /// </summary>
        public long MaxSnapshotFileSize = 1 * 1024 * 1024;
    }
}