using System;
using System.Collections.Generic;
using System.IO;

namespace Diffstore.IO
{
    /// <summary>
    /// Abstract class for storage backend implementations
    /// </summary>
    public abstract class IStorageBackend<TStorageOptions, TEntityOptions, TTableOptions>
    {
        protected TStorageOptions _storageOptions;
        protected TEntityOptions _entityOptions;
        protected TTableOptions _tableOptions;

        /// <summary>
        /// Passes parameters to base class
        /// </summary>
        /// <param name="storageOptions"></param>
        /// <param name="entityOptions"></param>
        /// <param name="tableOptions"></param>
        protected IStorageBackend(
            TStorageOptions storageOptions,
            TEntityOptions entityOptions,
            TTableOptions tableOptions)
            =>
            (_storageOptions, _entityOptions, _tableOptions) =
            (storageOptions, entityOptions, tableOptions);

        /// <summary>
        /// Retrieves all saved entity keys
        /// </summary>
        public abstract T[] GetAllKeys<T>();

        /// <summary>
        /// Begins raw entity data reading from storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Stream ready to be deserialized</returns>
        public abstract Stream BeginReadEntity<T>(T key);

        /// <summary>
        /// Begins raw entity data writing to storage
        /// </summary>
        /// <param name="key">Entity unique key</param>
        /// <returns>Empty stream for serialization</returns>
        public abstract Stream BeginWriteEntity<T>(T key);

        /// <summary>
        /// Deletes entity data and corresponding tables
        /// </summary>
        /// <param name="key">Entity unique key</param>
        public abstract void DeleteEntity<T>(T key);

        /// <summary>
        /// Begins an 'append to table' operation (like SQL INSERT)
        /// </summary>
        /// <param name="entityKey">Entity for which data will be appended</param>
        /// <param name="tableName">Target table name</param>
        /// <returns></returns>
        public abstract Stream BeginTableAppend<T>(T entityKey, string tableName);

        /// <summary>
        /// Opens raw stream containing entry with specified table key
        /// </summary>
        /// <param name="entityKey">Entity for which data will be written</param>
        /// <param name="tableName"></param>
        /// <param name="tableKey"></param>
        /// <returns></returns>
        public abstract Stream BeginTableReadWrite<TEntity, TTable>(TEntity entityKey, string tableName, 
            TTable tableKey) where TTable : IComparable<TTable>;
    }
}