using Microsoft.Extensions.Caching.Memory;

namespace Diffstore.Caching
{
    public class InMemoryCacheOptions
    {
        
        /// <summary>
        /// Gets or sets global entity cache options
        /// </summary>
        public MemoryCacheOptions GlobalOptions { get; set; }

        /// <summary>
        /// Gets or sets per-entry cache options
        /// </summary>
        public MemoryCacheEntryOptions EntryOptions { get; set; }
    }
}