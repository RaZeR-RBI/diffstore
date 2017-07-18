using System;

namespace Diffstore.Entities
{
    public interface IEntityReaderWriter<TKey, TInput, TOutput> : IDisposable
        where TKey : IComparable
    {        
        /// <summary>
        /// Retrieves all saved entity keys
        /// </summary>
        TKey[] GetAllKeys();

        /// <summary>
        /// Checks if an entity with the specified key exists
        /// </summary>
        bool Exists(TKey key);

        /// <summary>
        /// Begins raw entity data reading from storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Stream ready to be deserialized</returns>
        TInput BeginRead(TKey key);

        /// <summary>
        /// Begins raw entity data writing to storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Stream for serialization</returns>
        TOutput BeginWrite(TKey key);

        /// <summary>
        /// Deletes entity data and corresponding tables
        /// </summary>
        /// <param name="key">Entity unique key</param>
        void Drop(TKey key);
    }
}