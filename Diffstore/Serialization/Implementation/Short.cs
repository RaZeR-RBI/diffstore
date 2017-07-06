using System;
using System.IO;

namespace Diffstore.Serialization.Implementation
{
    public class ShortSerializer : ITypeSerializer<short>
    {
        public void Serialize(short input, Stream stream)
        {
            var bytes = BitConverter.GetBytes(input);
            stream.WriteByte(bytes[0]);
            stream.WriteByte(bytes[1]);
        }
    }

    public class ShortDeserializer : ITypeDeserializer<short>
    {
        public short Deserialize(Stream input)
        {
            var byte1 = input.ReadByte();
            var byte2 = input.ReadByte();
            return (short)((byte1 << 8) + byte2);
        }
    }

#if !CLS
    public class UShortSerializer : ITypeSerializer<ushort>
    {
        public void Serialize(ushort input, Stream stream)
        {
            var converted = unchecked((short)input);
            stream.WriteByte((byte)(converted >> 8));
            stream.WriteByte((byte)(converted & 255));
        }
    }

    public class UShortDeserializer : ITypeDeserializer<ushort>
    {
        public ushort Deserialize(Stream input)
        {
            var byte1 = input.ReadByte();
            var byte2 = input.ReadByte();
            return unchecked((ushort)((byte1 << 8) + byte2));
        }
    }
#endif
}