using System;
using System.IO;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    // TODO: Add more types
    public class FastBinaryFormatterTest : IDisposable
    {

        private MemoryStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private FastBinaryFormatter formatter = FastBinaryFormatter.Instance;

        public FastBinaryFormatterTest()
        {
            stream = new MemoryStream();
            reader = new BinaryReader(stream);
            writer = new BinaryWriter(stream);
        }

        public void Dispose()
        {
            writer.Close();
            writer.Dispose();
            reader.Close();
            reader.Dispose();
            stream.Dispose();
        }


        [Theory]
        [InlineData(0)]
        [InlineData(255)]
        [InlineData(123)]
        public void ShouldHandleByte(byte expected)
        {
            formatter.Serialize(expected, writer);
            stream.Seek(0, SeekOrigin.Begin);
            var actual = formatter.Deserialize(typeof(byte), reader);
            Assert.Equal(expected, actual);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(short.MinValue)]
        [InlineData(short.MaxValue)]
        [InlineData(1337)]
        public void ShouldHandleShort(short expected)
        {
            formatter.Serialize(expected, writer);
            stream.Seek(0, SeekOrigin.Begin);
            var actual = formatter.Deserialize(typeof(short), reader);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(int.MinValue)]
        [InlineData(int.MaxValue)]
        [InlineData(1337)]
        public void ShouldHandleInt(int expected)
        {
            formatter.Serialize(expected, writer);
            stream.Seek(0, SeekOrigin.Begin);
            var actual = formatter.Deserialize(typeof(int), reader);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(long.MinValue)]
        [InlineData(long.MaxValue)]
        [InlineData(1337)]
        public void ShouldHandleLong(long expected)
        {
            formatter.Serialize(expected, writer);
            stream.Seek(0, SeekOrigin.Begin);
            var actual = formatter.Deserialize(typeof(long), reader);
            Assert.Equal(expected, actual);
        }
    }
}