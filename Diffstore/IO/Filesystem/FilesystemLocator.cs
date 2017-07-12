using System;
using System.IO;
using static System.Math;
using static System.IO.Path;

namespace Diffstore.IO.Filesystem
{
    public static class FilesystemLocator
    {

        public static string LocateEntityFile(object key, FilesystemEntityStorageOptions options)
        {
            return Path.Combine(options.BasePath, FindEntitySubfolder(key, options), "entity");
        }

        // ugly but does the job
        private static string FindEntitySubfolder(object key, FilesystemEntityStorageOptions options)
        {
            var step = options.EntitiesPerDirectory;
            switch (key)
            {
                case byte b: return Partition(b, step);
                case short s: return Partition(s, step);
                case int i: return Partition(i, step);
                case long l: return Partition(l, step);
#if !CLS
                case sbyte sb: return Partition(sb, step);
                case ushort us: return Partition(us, step);
                case uint ui: return Partition(ui, step);
                case ulong ul: return Partition(ul, step);
#endif
                default: return key.GetHashCode().ToString();
            }
        }

        private static string Partition(long key, int step)
        {
            var subfolder = step > 0 ? ((long)Floor(key / (double)step)).ToString() : "";
            return Combine(subfolder, key.ToString());
        }

#if !CLS
        private static string Partition(ulong key, int step)
        {
            var subfolder = step > 0 ? ((ulong)Floor(key / (double)step)).ToString() : "";
            return Combine(subfolder, key.ToString());
        }
#endif
    }
}