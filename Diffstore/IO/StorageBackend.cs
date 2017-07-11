using System;
using System.Collections.Generic;
using System.IO;
using Diffstore.Entity;

namespace Diffstore.IO
{
    public class StorageBackend<TKey, TInput, TOutput> : IStorageBackend<TKey, TInput, TOutput>
    {
        protected IEntityReaderWriter<TInput, TOutput> entityIO;
        protected ITableReaderWriter<TInput, TOutput> tableIO;

        protected StorageBackend(
            IEntityReaderWriter<TInput, TOutput> entityReaderWriter, 
            ITableReaderWriter<TInput, TOutput> tableReaderWriter)
            => (entityIO, tableIO) = (entityReaderWriter, tableReaderWriter);

        public TKey[] GetAllKeys()
        {
            throw new NotImplementedException();
        }

        public TInput BeginReadEntity(TKey key)
        {
            throw new NotImplementedException();
        }

        public TOutput BeginWriteEntity(TKey key)
        {
            throw new NotImplementedException();
        }

        public void DropEntity(TKey key)
        {
            throw new NotImplementedException();
        }

        public TOutput BeginTableAppend(object entityKey, string tableName)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromTable(object entityKey, string tableName)
        {
            throw new NotImplementedException();
        }

        public void DeleteFromTables(string tableName)
        {
            throw new NotImplementedException();
        }
    }
}