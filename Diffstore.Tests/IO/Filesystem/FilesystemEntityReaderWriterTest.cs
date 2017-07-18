using System;
using System.IO;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Moq;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.IO.Filesystem
{
    public class FilesystemEntityReaderWriterTest : IDisposable
    {
        private readonly FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter> entityIO;

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

        [Fact]
        public void ShouldNotReadKeyfileIfKeyIsInPath()
        {
            var key = 1L;
            var filesystem = new MemoryFileSystem();
            var options = new FilesystemEntityStorageOptions();
            var mockFormatter = new Mock<IFormatter<BinaryReader, BinaryWriter>>();
            
            var entityIO = new FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter>(
                filesystem,
                mockFormatter.Object,
                options
            );

            using (var stream = entityIO.BeginWrite(key)) stream.Write("mock");
            Assert.True(entityIO.Exists(key));
            var keys = entityIO.GetAllKeys();
            Assert.Contains(key, keys);
            Assert.Single(keys);

            mockFormatter.Verify(mock => mock.Deserialize(
                It.IsAny<Type>(), 
                It.IsAny<BinaryReader>()
            ), Times.Never());
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