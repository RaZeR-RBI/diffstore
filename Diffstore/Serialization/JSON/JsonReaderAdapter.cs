using System;
using System.IO;
using System.Text;
using Diffstore.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Diffstore.Serialization.JSON
{
    internal class JsonReaderAdapter : IDisposable
    {
        private JObject _instance;
        public JObject Instance => _instance;
        
        public JsonReaderAdapter(Stream stream) {
            var content = Encoding.UTF8.GetString(stream.ReadAllBytes());
            _instance = JObject.Parse(content);
        }

        public void Dispose() => _instance = null;
    }
}