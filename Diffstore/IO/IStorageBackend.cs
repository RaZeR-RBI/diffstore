namespace Diffstore.IO
{
    public interface IStorageBackend<TKey, TInput, TOutput>
    {
        /// <summary>
        /// Retrieves all saved entity keys
        /// </summary>
        TKey[] GetAllKeys();

        /// <summary>
        /// Begins raw entity data reading from storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Stream ready to be deserialized</returns>
        TInput BeginReadEntity(TKey key);

        /// <summary>
        /// Begins raw entity data writing to storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Stream for serialization</returns>
        TOutput BeginWriteEntity(TKey key);

        /// <summary>
        /// Deletes entity data and corresponding tables
        /// </summary>
        /// <param name="key">Entity unique key</param>
        void DropEntity(TKey key);

        /// <summary>
        /// Begins an 'append to table' operation (like SQL INSERT)
        /// </summary>
        /// <param name="entityKey">Entity for which data will be appended</param>
        /// <param name="tableName">Target table name</param>
        /// <returns></returns>
        TOutput BeginTableAppend(object entityKey, string tableName);

        /// <summary>
        /// Removes all data from the specified table for the specified entity
        /// </summary>
        /// <param name="entityKey">Entity for which the specified table will be cleared</param>
        /// <param name="tableName">Target table name</param>
        void DeleteFromTable(object entityKey, string tableName);

        /// <summary>
        /// Removes all data from the specified table for all entities
        /// </summary>
        /// <param name="tableName">Target table name</param>
        void DeleteFromTables(string tableName);
    }
}