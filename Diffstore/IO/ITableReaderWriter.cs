using System;

namespace Diffstore.IO
{
    public interface ITableReaderWriter<TEntityKey, TInput, TOutput> : IDisposable
        where TEntityKey : IComparable
    {

        /// <summary>
        /// Begins a table read operation
        /// </summary>
        /// <param name="entityKey">Entity for which data should be retrieved</param>
        /// <param name="tableName">Target table name</param>
        /// <param name="startTime">Starting position</param>
        /// <returns></returns>
        TInput BeginRead(TEntityKey entityKey, string tableName, long startTime);

        /// <summary>
        /// Begins an 'append to table' operation
        /// </summary>
        /// <param name="entityKey">Entity for which data will be appended</param>
        /// <param name="tableName">Target table name</param>
        /// <returns></returns>
        TOutput BeginTableAppend(TEntityKey entityKey, string tableName);

        /// <summary>
        /// Removes all data from the specified table for the specified entity
        /// </summary>
        /// <param name="entityKey">Entity for which the specified table will be cleared</param>
        /// <param name="tableName">Target table name</param>
        void TruncateTable(TEntityKey entityKey, string tableName);
    }
}