using System.IO;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class BasicSerializationTest
    {
        [Fact]
        public void ByteSerializer()
        {
            var stream = new MemoryStream(32);
            var serializer = Serializer.For<byte>();
            var deserializer = Deserializer.For<byte>();

            byte expected = 123;
            serializer.Serialize(expected, stream);
            stream.Seek(0, SeekOrigin.Begin);
            var actual = deserializer.Deserialize(stream);

            Assert.Equal(expected, actual);
        }
    }
}