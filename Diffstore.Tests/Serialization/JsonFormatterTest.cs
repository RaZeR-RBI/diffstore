using Diffstore.Serialization;
using Diffstore.Serialization.JSON;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class JsonFormatterTest : IFormatterTest<JsonReaderAdapter, JsonWriterAdapter>
    {
        protected override IFormatter<JsonReaderAdapter, JsonWriterAdapter> Build()
            => JsonFormatter.Instance;

        [Fact]
        public override void Test() => base.Test();
    }
}