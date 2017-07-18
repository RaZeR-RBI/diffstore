using System;
using System.IO;
using static System.Math;
using static System.IO.Path;
using SharpFileSystem;
using System.Collections.Generic;

namespace Diffstore.Entities.Filesystem
{
    public static class FilesystemLocator
    {
        public const string EntityFilename = "entity";
        public const string KeyFilename = "keydata";

        static readonly Dictionary<Type, Func<string, object>> mappers =
            new Dictionary<Type, Func<string, object>>
            {
                { typeof(byte), (path) => byte.Parse(path) },
                { typeof(short), (path) => short.Parse(path) },
                { typeof(int), (path) => int.Parse(path) },
                { typeof(long), (path) => long.Parse(path) },
                #if !CLS
                { typeof(sbyte), (path) => sbyte.Parse(path) },
                { typeof(ushort), (path) => ushort.Parse(path) },
                { typeof(uint), (path) => uint.Parse(path) },
                { typeof(ulong), (path) => ulong.Parse(path) },
                #endif
            };

        public static FileSystemPath LocateEntityFile(object key, FilesystemEntityStorageOptions options)
        {
            return FindEntitySubfolder(key, options).AppendFile(EntityFilename);
        }

        public static FileSystemPath LocateKeyFile(object key, FilesystemEntityStorageOptions options)
        {
            return FindEntitySubfolder(key, options).AppendFile(KeyFilename);
        }

        public static bool IsKeyFile(FileSystemPath path)
        {
            return path.IsFile && path.EntityName == KeyFilename;
        }

        // ugly but does the job
        private static FileSystemPath FindEntitySubfolder(object key, FilesystemEntityStorageOptions options)
        {
            var step = options.EntitiesPerDirectory;
            var root = options.BasePath;
            switch (key)
            {
                case byte b: return root.Partition(b, step);
                case short s: return root.Partition(s, step);
                case int i: return root.Partition(i, step);
                case long l: return root.Partition(l, step);
#if !CLS
                case sbyte sb: return root.Partition(sb, step);
                case ushort us: return root.Partition(us, step);
                case uint ui: return root.Partition(ui, step);
                case ulong ul: return root.Partition(ul, step);
#endif
                default: return root.AppendDirectory(key.GetHashCode().ToString());
            }
        }

        private static FileSystemPath Partition(this FileSystemPath path, long key, int step)
        {
            var result = path;
            if (step > 0)
            {
                var subfolder = ((long)Floor(key / (double)step)).ToString();
                result = result.AppendDirectory(subfolder);
            }
            return result.AppendDirectory(key.ToString());
        }

        public static TKey ExtractKey<TKey>(this FileSystemPath path)
        {
            var type = typeof(TKey);
            var folder = path.ParentPath.EntityName;
            if (!mappers.ContainsKey(type)) return default(TKey);
            return (TKey)mappers[type].Invoke(folder);
        }

#if !CLS
        private static FileSystemPath Partition(this FileSystemPath path, ulong key, int step)
        {
            var result = path;
            if (step > 0)
            {
                var subfolder = ((ulong)Floor(key / (double)step)).ToString();
                result = result.AppendDirectory(subfolder);
            }
            return result.AppendDirectory(key.ToString());
        }
#endif
    }
}