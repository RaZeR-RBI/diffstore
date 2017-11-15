using System;
using System.Collections.Generic;
using System.Linq;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Diffstore.Serialization.File;
using SharpFileSystem;

namespace Diffstore.Snapshots.Filesystem
{
    public class SingleFileSnapshotManager<TKey, TValue, TIn, TOut> : 
        ISnapshotManager<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {
        private readonly FilesystemStorageOptions options;
        private readonly IFileSystem fileSystem;
        private readonly IFileFormatter<TIn, TOut> formatter;
        private readonly Schema schema = SchemaManager.Get(typeof(TValue));

        public SingleFileSnapshotManager(FilesystemStorageOptions opts,
            IFileFormatter<TIn, TOut> format,
            IFileSystem fs) =>
            (options, fileSystem, formatter) = (opts, fs, format);

        public bool Any(TKey key) => GetExistingFiles(key).Any();

        public void Dispose() {
            formatter.Dispose();
            fileSystem.Dispose();
        }

        public void Drop(TKey entityKey) =>
            GetExistingFiles(entityKey).ToList()
                .ForEach(p => fileSystem.Delete(p));

        public IEnumerable<Snapshot<TKey, TValue>> GetAll(TKey key) =>
            GetExistingFiles(key).Select(file => Read(key, file));

        public Snapshot<TKey, TValue> GetFirst(TKey key) =>
            Read(key, GetExistingFiles(key).First());

        public long GetFirstTime(TKey key) =>
            TimeFromFilename(GetExistingFiles(key).First());

        public IEnumerable<Snapshot<TKey, TValue>> GetInRange(TKey key, long timeStart, long timeEnd) =>
            GetExistingFiles(key)
                .SkipWhile(p => TimeFromFilename(p) < timeStart)
                .TakeWhile(p => TimeFromFilename(p) < timeEnd)
                .Select(p => Read(key, p));

        public Snapshot<TKey, TValue> GetLast(TKey key) =>
            Read(key, GetExistingFiles(key).Last());

        public long GetLastTime(TKey key) =>
            TimeFromFilename(GetExistingFiles(key).Last());

        public IEnumerable<Snapshot<TKey, TValue>> GetPage(TKey key, int from, int count, 
            bool ascending = true) =>
            GetExistingFiles(key, ascending)
                .Skip(from)
                .Take(count)
                .Select(p => Read(key, p));

        public void Make(Entity<TKey, TValue> entity) =>
            Make(entity, Snapshot.GetCurrentUnixSeconds());

        public void Make(Entity<TKey, TValue> entity, long time)
        {
            var path = FilesystemLocator.FindSnapshotFolder(entity.Key, options)
                .AppendFile(time.ToString());

            if (!fileSystem.Exists(path.ParentPath))
                fileSystem.CreateDirectoryRecursive(path.ParentPath);
                
            var writer = formatter.BeginWrite(fileSystem, path);
            foreach(var field in schema.Fields)
            {
                if (field.IgnoreChanges) continue;
                var fieldValue = field.Getter(entity.Value);
                if (fieldValue == null) continue;
                formatter.Serialize(fieldValue, writer, field.Name);
            }
            formatter.EndWrite(writer);
        }

        private Snapshot<TKey, TValue> Read(TKey key, FileSystemPath path)
        {
            var time = long.Parse(path.EntityName);
            var reader = formatter.BeginRead(fileSystem, path);

            var instance = new TValue();
            foreach(var field in schema.Fields)
            {
                var value = formatter.Deserialize(field.Type, reader, field.Name);
                if (value == null) continue;
                field.Setter(instance, value);
            }
            return Snapshot.Create(time, Entity.Create(key, instance));
        }

        private IOrderedEnumerable<FileSystemPath> GetExistingFiles(TKey key, bool ascending = true)
        {
            var folder = FilesystemLocator.FindSnapshotFolder(key, options);
            if (!fileSystem.Exists(folder)) fileSystem.CreateDirectoryRecursive(folder);
            return fileSystem.GetEntities(folder)
                .Where(path => path.IsFile)
                .Where(path => fileSystem.Exists(path))
                .OrderBy(x => TimeFromFilename(x) * (ascending ? 1 : -1));
        }

        private long TimeFromFilename(FileSystemPath path) => long.Parse(path.EntityName);
    }
}