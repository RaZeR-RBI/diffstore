using Xunit;

namespace Diffstore.Tests.Serialization
{
    #if !CLS
    public class NonCLSSerializationTest
    {
        [Theory]
        [InlineData(0)]
        [InlineData(-128)]
        [InlineData(127)]
        [InlineData(12)]
        public void SByteSerializer(sbyte expected)
        {
            TestUtil.Setup<sbyte>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(65535)]
        [InlineData(1337)]
        public void UShortSerializer(ushort expected)
        {
            TestUtil.Setup<ushort>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(65535)]
        [InlineData(1337)]
        public void UIntSerializer(uint expected)
        {
            TestUtil.Setup<uint>(out var stream, out var serializer, out var deserializer);
            var actual = TestUtil.ProcessValue(expected, serializer, deserializer, stream);
            Assert.True(expected == actual);
        }
    }
    #endif
}