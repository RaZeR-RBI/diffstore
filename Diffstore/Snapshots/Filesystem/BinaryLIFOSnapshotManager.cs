using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Diffstore.Utils;
using SharpFileSystem;

namespace Diffstore.Snapshots.Filesystem
{
    /*
        File name:
        startTime (long) in base32 + '-' + file size (int) in base32

        File layout:
        ---- header ----
        (0 ... N bytes) - zero bytes for padding
        (byte) 255 - marker

        ---- for each snapshot ---
        (long) - time
        (VLQ int) - data length
        (bytes) - data
        ----
     */
    public class BinaryLIFOSnapshotManager<TKey, TValue> : ISnapshotManager<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {

        private readonly FilesystemStorageOptions options;
        private readonly IFileSystem fileSystem;
        private readonly IFormatter<BinaryReader, BinaryWriter> formatter =
            FastBinaryFormatter.Instance;
        private readonly Schema schema = SchemaManager.Get(typeof(TValue));
        private readonly int zeroPaddingStep;

        public BinaryLIFOSnapshotManager(FilesystemStorageOptions opts, 
            IFileSystem fs, int maxPadding)
        {
            (options, fileSystem, zeroPaddingStep) = (opts, fs, maxPadding);
            if (options.MaxSnapshotFileSize % zeroPaddingStep != 0)
                throw new ArgumentException(
                    "MaxSnapshotFileSize must be a multiple of maxPadding");
        }

        public void Dispose()
        {
            fileSystem.Dispose();
        }

        public void Drop(TKey entityKey)
        {
            var folder = FilesystemLocator.FindSnapshotFolder(entityKey, options);
            var entities = fileSystem.GetEntitiesRecursive(folder).ToList();
            foreach (var entity in entities) fileSystem.Delete(entity);
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetAll(TKey key)
        {
            return GetExistingFiles(key)
                .Reverse()
                .Select(file => ReadAllFrom(key, file))
                .SelectMany(i => i);
        }

        public Snapshot<TKey, TValue> GetFirst(TKey key)
        {
            return ReadAllFrom(key,
                GetExistingFiles(key).First()).Last();
        }

        public long GetFirstTime(TKey key)
        {
            var path = GetExistingFiles(key).First();
            FromFilename(path.EntityName, out long result, out int unused);
            return result;
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetInRange(TKey key, long timeStart, long timeEnd)
        {
            return GetAll(key)
                .SkipWhile(x => x.Time >= timeEnd)
                .TakeWhile(x => x.Time >= timeStart);
        }

        public Snapshot<TKey, TValue> GetLast(TKey key)
        {
            var path = GetExistingFiles(key).Last();
            var stream = OpenReadFromStart(path, out int origin);
            using (var br = new BinaryReader(stream))
                return ReadSingle(key, br);
        }

        public long GetLastTime(TKey key)
        {
            var path = GetExistingFiles(key).Last();
            var stream = OpenReadFromStart(path, out int origin);
            using (var br = new BinaryReader(stream))
                return br.ReadInt64();
        }

        public IEnumerable<Snapshot<TKey, TValue>> GetPage(TKey key, int from, int count,
            bool ascending)
        {
            var source = ascending ? GetAll(key).OrderBy(i => i.Time) : GetAll(key);
            return source.Skip(from).Take(count);
        }

        public void Make(Entity<TKey, TValue> entity)
        {
            Make(entity, Snapshot.GetCurrentUnixSeconds());
        }

        public void Make(Entity<TKey, TValue> entity, long time)
        {
            var key = entity.Key;
            var data = Write(Snapshot.Create(time, entity));

            var existingFiles = GetExistingFiles(key);
            var fileStream = existingFiles.Any() ?
                UseExistingFileIfPossible(existingFiles.Last(), key, time, data.Length) :
                CreateNewFile(key, time, data.Length);

            using (fileStream)
                fileStream.Write(data, 0, data.Length);
        }

        private Stream UseExistingFileIfPossible(FileSystemPath path, TKey key,
            long time, int requiredEmptyBytes)
        {
            FromFilename(path.EntityName, out long startTime, out int fileSize);
            var maxSize = options.MaxSnapshotFileSize;
            if (maxSize > 0)
            {
                if (requiredEmptyBytes >= zeroPaddingStep)
                    throw new ArgumentException(
                        "Snapshot data cannot be bigger than zero byte padding size");
            }

            var stream = OpenReadFromStart(path, out int bytesRemaining);
            if (requiredEmptyBytes > bytesRemaining)
            {
                stream.Close();
                return (fileSize + zeroPaddingStep > maxSize) ?
                    CreateNewFile(key, time, requiredEmptyBytes) :
                    Rewrite(path, key, time, requiredEmptyBytes);
            }
            else
                return SeekOrReopen(stream, path, bytesRemaining, requiredEmptyBytes);
        }

        private Stream Rewrite(FileSystemPath path, TKey key, long time,
            int requiredEmptyBytes)
        {
            FromFilename(path.EntityName, out long startTime, out int fileSize);
            fileSize += zeroPaddingStep;
            var data = OpenReadFromStart(path, out int origin).ReadAllBytes();
            var prefix = new byte[zeroPaddingStep];

            fileSystem.Delete(path);
            
            var newPath = path.ParentPath.AppendFile(ToFilename(startTime, fileSize));
            var stream = fileSystem.CreateFile(newPath);
            stream.Write(prefix, 0, zeroPaddingStep);
            stream.Write(prefix, 0, origin + 1);
            stream.Write(data, 0, data.Length);
            return SeekOrReopen(stream, newPath, origin + zeroPaddingStep, requiredEmptyBytes);
        }

        private Stream SeekOrReopen(Stream stream, FileSystemPath path, int bytesRemaining,
            int requiredEmptyBytes)
        {
            var target = bytesRemaining - requiredEmptyBytes + 1;
            if (stream.CanSeek)
            {
                stream.Seek(target, SeekOrigin.Begin);
                return stream;
            }
            else
            {
                stream.Close();
                var newStream = fileSystem.OpenFile(path, FileAccess.ReadWrite);
                if (target > 0)
                {
                    byte[] tmp = new byte[target];
                    newStream.Read(tmp, 0, target);
                }
                return newStream;
            }
        }

        private Stream CreateNewFile(TKey key, long time, int requiredEmptyBytes)
        {
            var fileName = ToFilename(time, zeroPaddingStep);
            var filePath = FilesystemLocator.FindSnapshotFolder(key, options)
                .AppendFile(fileName);
            using (var fileStream = fileSystem.CreateFile(filePath))
            {
                var data = new byte[zeroPaddingStep];
                data[zeroPaddingStep - 1] = 255;
                fileStream.Write(data, 0, zeroPaddingStep);
            }
            var stream = fileSystem.OpenFile(filePath, FileAccess.ReadWrite);
            return SeekOrReopen(stream, filePath, zeroPaddingStep - 1, requiredEmptyBytes);
        }

        private byte[] Write(Snapshot<TKey, TValue> snapshot)
        {
            var value = snapshot.State.Value;
            using (var stream = new MemoryStream())
            using (var output = new BinaryWriterWith7Bit(stream))
            {
                foreach (var field in schema.Fields)
                {
                    if (field.IgnoreChanges) continue;
                    var fieldValue = field.Getter(value);
                    formatter.Serialize(fieldValue, output);
                }
                var dataLength = (int)stream.Length;
                output.Write((byte)255);
                output.Write(snapshot.Time);
                output.Write7BitEncodedInt(dataLength);
                var prefixLength = stream.Length - dataLength;

                var src = stream.ToArray();
                var dest = new byte[prefixLength + dataLength];
                Array.Copy(src, dataLength, dest, 0, prefixLength);
                Array.Copy(src, 0, dest, prefixLength, dataLength);
                return dest;
            }
        }

        private Stream OpenReadFromStart(FileSystemPath path, out int bytesRemaining)
        {
            var stream = fileSystem.OpenFile(path, FileAccess.ReadWrite);
            MoveToStart(stream, out int bytes);
            bytesRemaining = bytes;
            return stream;
        }

        private void MoveToStart(Stream stream, out int bytesRemaining)
        {
            bytesRemaining = -1;
            do { bytesRemaining++; } while (stream.ReadByte() == 0);
        }

        private IOrderedEnumerable<FileSystemPath> GetExistingFiles(TKey key)
        {
            var folder = FilesystemLocator.FindSnapshotFolder(key, options);
            if (!fileSystem.Exists(folder)) fileSystem.CreateDirectoryRecursive(folder);
            return fileSystem.GetEntities(folder)
                .Where(path => path.IsFile)
                .Where(path => fileSystem.Exists(path))
                .OrderBy(x => TimeFromFilename(x));
        }

        private void FromFilename(string name, out long startTime, out int fileSize)
        {
            var segments = name.Split('-');
            startTime = BitConverter.ToInt64(Base32Encoding.ToBytes(segments[0]), 0);
            fileSize = BitConverter.ToInt32(Base32Encoding.ToBytes(segments[1]), 0);
        }

        private string ToFilename(long startTime, int fileSize)
        {
            return Base32Encoding.ToString(BitConverter.GetBytes(startTime)) +
                "-" + Base32Encoding.ToString(BitConverter.GetBytes(fileSize));
        }

        private long TimeFromFilename(FileSystemPath path)
        {
            var bytes = Base32Encoding.ToBytes(path.EntityName.Split('-')[0]);
            return BitConverter.ToInt32(bytes, 0);
        }

        private IEnumerable<Snapshot<TKey, TValue>> ReadAllFrom(TKey key,
            FileSystemPath path)
        {
            var data = OpenReadFromStart(path, out int origin).ReadAllBytes();
            using (var ms = new MemoryStream(data, false))
            using (var br = new BinaryReader(ms))
                do yield return ReadSingle(key, br); while (ms.Position != ms.Length);
        }

        private Snapshot<TKey, TValue> ReadSingle(TKey key, BinaryReader reader)
        {
            var time = reader.ReadInt64();
            // skipping the length
            byte lengthByte;
            do
            {
                lengthByte = reader.ReadByte();
            } while ((lengthByte & (1 << 7)) != 0);

            TValue value = new TValue();
            foreach (var field in schema.Fields)
            {
                if (field.IgnoreChanges) continue;
                var fieldValue = formatter.Deserialize(field.Type, reader);
                if (fieldValue == null) continue;
                field.Setter(value, fieldValue);
            }
            return Snapshot.Create(time, Entity.Create(key, value));
        }

        public bool Any(TKey key)
        {
            return GetExistingFiles(key).Any();
        }
    }
}