using System;
using System.IO;

namespace Diffstore.Serialization.Implementation
{
    public class ByteDeserializer : ITypeDeserializer<byte>
    {
        public byte Deserialize(Stream input)
        {
            var result = input.ReadByte();
            if (result == -1) throw new EndOfStreamException();
            return (byte)result;
        }
    }
}