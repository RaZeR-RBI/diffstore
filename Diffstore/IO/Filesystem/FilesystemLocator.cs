using System;
using System.IO;
using static System.Math;
using static System.IO.Path;
using SharpFileSystem;

namespace Diffstore.IO.Filesystem
{
    public static class FilesystemLocator
    {
        public const string EntityFilename = "entity";
        public const string KeyFilename = "keydata";

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