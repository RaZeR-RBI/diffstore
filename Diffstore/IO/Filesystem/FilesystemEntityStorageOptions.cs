using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Diffstore.IO.Filesystem
{
    public class FilesystemEntityStorageOptions
    {
        /// <summary>
        /// Gets or sets the root directory for the storage backend.
        /// Defaults to current directory.
        /// </summary>
        public string BasePath { get; set; } = "";

        /// <summary>
        /// Specifies entity per directory count (0 means that all entities 
        /// will be in one directory).
        /// </summary>
        public int EntitiesPerDirectory { get; set; } = 1000;
    }
}