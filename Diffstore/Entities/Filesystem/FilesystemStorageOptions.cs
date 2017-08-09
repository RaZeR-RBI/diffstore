using System;
using SharpFileSystem;

namespace Diffstore.Entities.Filesystem
{
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