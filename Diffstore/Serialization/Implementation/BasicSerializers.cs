using System;
using System.IO;

namespace Diffstore.Serialization.Implementation
{
    public class ByteSerializer : ITypeSerializer<byte>
    {
        public void Serialize(byte input, Stream stream)
        {
            stream.WriteByte(input);
        }
    }
}