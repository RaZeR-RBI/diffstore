using System;
using System.IO;
using System.Linq;
using Diffstore.Serialization;
using Diffstore.Utils;
using SharpFileSystem;

/// <summary>
/// Filesystem entity storage related types
/// </summary>
namespace Diffstore.Entities.Filesystem
{
    internal class FilesystemEntityReaderWriter<TKey, TInput, TOutput> :
        IEntityReaderWriter<TKey, TInput, TOutput>
        where TKey : IComparable
        where TInput: IDisposable
        where TOutput : IDisposable
    {
        protected readonly IFileSystem filesystem;
        protected readonly IFormatter<TInput, TOutput> formatter;
        protected readonly FilesystemStorageOptions options;

        public FilesystemEntityReaderWriter(
            IFileSystem filesystem,
            IFormatter<TInput, TOutput> formatter,
            FilesystemStorageOptions options)
            =>
            (this.filesystem, this.formatter, this.options) =
            (filesystem, formatter, options);

        public TInput BeginRead(TKey key)
        {
            var fileToRead = filesystem.OpenFile(
                FilesystemLocator.LocateEntityFile(key, options),
                FileAccess.ReadWrite);

            return StreamBuilder.FromStream<TInput>(fileToRead);
        }

        public TOutput BeginWrite(TKey key)
        {
            var path = FilesystemLocator.LocateEntityFile(key, options);
            if (filesystem.Exists(path)) filesystem.Delete(path);
            var fileToWrite = CreateDirectoriesAndFile(key, path);
            return StreamBuilder.FromStream<TOutput>(fileToWrite);
        }

        private Stream CreateDirectoriesAndFile(object key, FileSystemPath path)
        {
            filesystem.CreateDirectoryRecursive(path.ParentPath);
            var keyfile = filesystem.CreateFile(FilesystemLocator.LocateKeyFile(key, options));
            using (var keyfileWriter = StreamBuilder.FromStream<TOutput>(keyfile))
                formatter.Serialize(key, keyfileWriter, "Key");
            
            return filesystem.CreateFile(path);
        }

        public void Dispose()
        {
            filesystem.Dispose();
        }

        public void Drop(TKey key)
        {
            var parentPath = FilesystemLocator.LocateEntityFile(key, options).ParentPath;
            var entities = filesystem.GetEntitiesRecursive(parentPath).ToList();
            foreach(var entity in entities)
                filesystem.Delete(entity);
        }

        public bool Exists(TKey key)
        {
            var path = FilesystemLocator.LocateKeyFile(key, options);
            return filesystem.Exists(path);
        }

        public TKey[] GetAllKeys()
        {
            if (!filesystem.Exists(options.BasePath)) filesystem.CreateDirectoryRecursive(options.BasePath);
            return filesystem.GetEntitiesRecursive(options.BasePath)
                .Where((entity) => FilesystemLocator.IsKeyFile(entity))
                .Select((file) => ReadKeyfile(file)).ToArray();
        }

        private TKey ReadKeyfile(FileSystemPath path)
        {
            TKey result = FilesystemLocator.ExtractKey<TKey>(path);
            if (!result.Equals(default(TKey))) return result;

            using (var keyfile = filesystem.OpenFile(path, FileAccess.Read))
            using (var keyfileReader = StreamBuilder.FromStream<TInput>(keyfile))
                return (TKey)formatter.Deserialize(typeof(TKey), keyfileReader, "Key");
        }
    }
}