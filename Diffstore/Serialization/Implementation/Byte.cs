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

    public class ByteDeserializer : ITypeDeserializer<byte>
    {
        public byte Deserialize(Stream input)
        {
            var result = input.ReadByte();
            if (result == -1) throw new EndOfStreamException();
            return (byte)result;
        }
    }

#if !CLS
    [CLSCompliant(false)]
    public class SByteSerializer : ITypeSerializer<sbyte>
    {
        public void Serialize(sbyte input, Stream stream)
        {
            stream.WriteByte(unchecked((byte)input));
        }
    }

    [CLSCompliant(false)]
    public class SByteDeserializer : ITypeDeserializer<sbyte>
    {
        public sbyte Deserialize(Stream input)
        {
            var result = input.ReadByte();
            if (result == -1) throw new EndOfStreamException();
            return unchecked((sbyte)result);
        }
    }
#endif
}