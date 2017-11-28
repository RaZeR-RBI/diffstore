using System;
using System.IO;
using Newtonsoft.Json;

namespace Diffstore.Serialization.JSON
{
    public class JsonWriterAdapter : IDisposable
    {
        private JsonWriter _instance;
        public JsonWriter Instance => _instance;
        
        public JsonWriterAdapter(Stream stream) {
            _instance = new JsonTextWriter(new StreamWriter(stream));
            _instance.CloseOutput = true;
            _instance.AutoCompleteOnClose = true;
            _instance.WriteStartObject();
        }

        public void Dispose() => _instance.Close();
    }
}