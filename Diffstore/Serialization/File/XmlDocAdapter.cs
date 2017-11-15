using System;
using System.IO;
using System.Xml;

namespace Diffstore.Serialization.File
{
    /* 
        Utility class to allow XmlDocument to be used in
        EntityManager and SnapshotManager classes.
     */
    public class XmlDocumentAdapter : IDisposable
    {
        private XmlDocument _document = new XmlDocument();

        public XmlDocument Instance
        {
            get => _document;
        }

        public XmlDocumentAdapter(Stream stream) {
            _document.Load(stream);
            stream.Close();
        }
        
        public void Dispose() => _document = null;
    }
}