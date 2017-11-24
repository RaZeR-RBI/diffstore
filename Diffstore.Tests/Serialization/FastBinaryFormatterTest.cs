using System.IO;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class FastBinaryFormatterTest : IFormatterTest<BinaryReader, BinaryWriter>
    {
        protected override IFormatter<BinaryReader, BinaryWriter> Build() =>
            FastBinaryFormatter.Instance;

        [Fact]
        public override void Test() => base.Test();
    }
}