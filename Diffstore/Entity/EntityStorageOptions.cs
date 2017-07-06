using Microsoft.Extensions.Caching.Memory;

namespace Diffstore.Entity
{
    public class EntityFileStorageOptions
    {
        public string BasePath { get; set; }
        /// <summary>
        /// Specifies entity per directory count (0 means all entities will be in one directory)
        /// For example, if you have 1M entities, it's better to put them to
        /// subdirectories with 100k entities each for filesystem performance improvement.
        /// </summary>
        public int EntityPartitioningStep { get; set; }
    }
}