using System;
using System.IO;
using System.Text;
using System.Xml;
using Diffstore.Utils;

namespace Diffstore.Serialization.File
{
    /* 
        Utility class to allow XmlDocument to be used in
        EntityManager and SnapshotManager classes.
     */
    public class XmlDocumentAdapter : IDisposable
    {
        private XmlDocument _document = new XmlDocument();

        private static readonly string _bomMark = Encoding.UTF8.GetString(
            Encoding.UTF8.GetPreamble()
        );

        public XmlDocument Instance
        {
            get => _document;
        }

        public XmlDocumentAdapter(Stream stream)
        {
            var contents = stream.ReadAllBytes();
            var xml = Defuse(Encoding.UTF8.GetString(contents));
            _document.LoadXml(xml);
            stream.Close();
        }

        // don't get BOMbed
        private static string Defuse(string input) =>
            input.IndexOf('<') > 0 ? input.Substring(input.IndexOf('<')) : input;

        public void Dispose() => _document = null;
    }
}