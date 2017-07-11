using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Diffstore.Entity
{
    public class EntityFileStorageOptions
    {
        /// <summary>
        /// Gets or sets the root directory for the storage backend.
        /// Defaults to current directory.
        /// </summary>
        public string BasePath { get; set; } = "";

        /// <summary>
        /// Specifies entity per directory count (0 means all entities will be in one directory)
        /// For example, if you have 1M entities, it's better to put them to
        /// subdirectories with 100k entities each for filesystem performance improvement.
        /// Zero means no subdivision.
        /// </summary>
        public int EntitiesPerDirectory { get; set; } = 2500;
    }
}