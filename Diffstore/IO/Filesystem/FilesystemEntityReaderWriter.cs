using System;
using Diffstore.Serialization;
using SharpFileSystem;

namespace Diffstore.IO.Filesystem
{
    public class FilesystemEntityReaderWriter<TKey, TInput, TOutput> :
        IEntityReaderWriter<TKey, TInput, TOutput>
        where TKey : IComparable
    {
        protected IFileSystem filesystem;
        protected IFormatter<TInput, TOutput> formatter;
        protected FilesystemEntityStorageOptions options;

        public FilesystemEntityReaderWriter(
            IFileSystem filesystem,
            IFormatter<TInput, TOutput> formatter,
            FilesystemEntityStorageOptions options)
            =>
            (this.filesystem, this.formatter, this.options) =
            (filesystem, formatter, options);

        public TInput BeginRead(TKey key)
        {
            throw new NotImplementedException();
        }

        public TOutput BeginWrite(TKey key)
        {
            throw new NotImplementedException();
        }

        public void Drop(TKey key)
        {
            throw new NotImplementedException();
        }

        public bool Exists(TKey key)
        {
            throw new NotImplementedException();
        }

        public TKey[] GetAllKeys()
        {
            throw new NotImplementedException();
        }
    }
}