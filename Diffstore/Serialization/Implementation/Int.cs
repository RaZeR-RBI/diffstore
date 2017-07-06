using System;
using System.IO;

namespace Diffstore.Serialization.Implementation
{
    public class IntSerializer : ITypeSerializer<int>
    {
        public void Serialize(int input, Stream stream)
        {
            stream.WriteByte((byte)(input >> 24));
            stream.WriteByte((byte)(input >> 16));
            stream.WriteByte((byte)(input >> 8));
            stream.WriteByte((byte)(input & 255));
        }
    }

    public class IntDeserializer : ITypeDeserializer<int>
    {
        public int Deserialize(Stream input)
        {
            var byte1 = input.ReadByte();
            var byte2 = input.ReadByte();
            var byte3 = input.ReadByte();
            var byte4 = input.ReadByte();
            return (int)((byte1 << 24) + (byte2 << 16) + (byte3 << 8) + byte4);
        }
    }

#if !CLS
    [CLSCompliant(false)]
    public class UIntSerializer : ITypeSerializer<uint>
    {
        public void Serialize(uint input, Stream stream)
        {
            var converted = unchecked((int)input);
            stream.WriteByte((byte)(converted >> 24));
            stream.WriteByte((byte)(converted >> 16));
            stream.WriteByte((byte)(converted >> 8));
            stream.WriteByte((byte)(converted & 255));
        }
    }

    [CLSCompliant(false)]
    public class UIntDeserializer : ITypeDeserializer<uint>
    {
        public uint Deserialize(Stream input)
        {
            var byte1 = input.ReadByte();
            var byte2 = input.ReadByte();
            var byte3 = input.ReadByte();
            var byte4 = input.ReadByte();
            return (int)((byte1 << 24) + (byte2 << 16) + (byte3 << 8) + byte4);
        }
    }
#endif
}