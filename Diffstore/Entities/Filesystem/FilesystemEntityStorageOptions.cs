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
    }
}