using Diffstore.Serialization;
using Diffstore.Serialization.XML;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class XmlFormatterTest : IFormatterTest<XmlDocumentAdapter, XmlWriterAdapter>
    {
        protected override IFormatter<XmlDocumentAdapter, XmlWriterAdapter> Build() =>
            XmlFormatter.Instance;

        [Fact]
        public override void Test() => base.Test();
    }
}