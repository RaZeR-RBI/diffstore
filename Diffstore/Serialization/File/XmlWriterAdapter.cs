using System;
using System.IO;
using System.Xml;

namespace Diffstore.Serialization.File
{
    public class XmlWriterAdapter : IDisposable
    {
        private XmlWriter _writer;
        public XmlWriter Instance
        {
            get => _writer;
        }

        public XmlWriterAdapter(Stream stream) {
            _writer = XmlWriter.Create(stream);
            _writer.WriteStartDocument();
            _writer.WriteStartElement("Value");
        }

        public void Dispose() => _writer.Close();
    }
}