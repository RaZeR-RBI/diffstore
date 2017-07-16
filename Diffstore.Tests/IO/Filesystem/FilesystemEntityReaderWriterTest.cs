using System;
using System.IO;
using Diffstore.IO.Filesystem;
using Diffstore.Serialization;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.IO.Filesystem
{
    public class FilesystemEntityReaderWriterTest : IDisposable
    {
        private FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter> entityIO;

        private const long KEY_RW = 1;
        private const long KEY_EXISTS = 2;
        private const long KEY_DROP = 3;
        private const long KEY_GET_ALL = 4;

        public FilesystemEntityReaderWriterTest()
        {
            var filesystem = new MemoryFileSystem();
            var options = new FilesystemEntityStorageOptions();
            entityIO = new FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter>(
                filesystem,
                FastBinaryFormatter.Instance,
                options
            );
        }

        public void Dispose()
        {
            entityIO.Dispose();
        }

        [Fact]
        public void ShouldExistAfterCreation()
        {
            WriteSampleStringData(KEY_EXISTS);
            var exists = entityIO.Exists(KEY_EXISTS);
            Assert.True(exists);
        }

        [Fact]
        public void ShouldReturnAllKeys()
        {
            WriteSampleStringData(KEY_GET_ALL);
            var allKeys = entityIO.GetAllKeys();
            Assert.Contains(KEY_GET_ALL, allKeys);
        }

        [Fact]
        public void ShouldDropIfExists()
        {
            Assert.False(entityIO.Exists(KEY_DROP));

            WriteSampleStringData(KEY_DROP);
            Assert.True(entityIO.Exists(KEY_DROP));

            entityIO.Drop(KEY_DROP);
            Assert.False(entityIO.Exists(KEY_DROP));
        }

        [Fact]
        public void ShouldReadWhatItWrote()
        {
            var expected = "test string";
            WriteSampleStringData(KEY_RW, expected);
            var actual = ReadString(KEY_RW);
            Assert.Equal(expected, actual);
        }

        private void WriteSampleStringData(long key, string data = "some data")
        {
            using (var stream = entityIO.BeginWrite(key)) stream.Write(data);
        }

        private string ReadString(long key)
        {
            var result = "";
            using (var stream = entityIO.BeginRead(key)) result = stream.ReadString();
            return result;
        }
    }
}