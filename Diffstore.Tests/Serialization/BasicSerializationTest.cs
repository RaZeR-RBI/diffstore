using System.IO;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class BasicSerializationTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(255)]
        [InlineData(123)]
        public void ByteSerializer(byte expected)
        {
            TestUtil.Setup<byte>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(-32768)]
        [InlineData(32767)]
        [InlineData(1337)]
        public void ShortSerializer(short expected)
        {
            TestUtil.Setup<short>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-2147483648)]
        [InlineData(2147483647)]
        [InlineData(1337)]
        public void IntSerializer(int expected)
        {
            TestUtil.Setup<int>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }
    }
}