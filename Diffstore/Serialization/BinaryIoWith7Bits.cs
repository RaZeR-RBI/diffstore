using System.IO;
using System.Text;

namespace Diffstore.Serialization
{
    public class BinaryReaderWith7Bit : BinaryReader
    {
        public BinaryReaderWith7Bit(Stream input) : 
            base(input) { }

        public BinaryReaderWith7Bit(Stream input, Encoding encoding) :
            base(input, encoding) { }
        public BinaryReaderWith7Bit(Stream input, Encoding encoding, bool leaveOpen) :
            base(input, encoding, leaveOpen) { }

        public new int Read7BitEncodedInt()
        {
            return base.Read7BitEncodedInt();
        }
    }

    public class BinaryWriterWith7Bit : BinaryWriter
    {
        public BinaryWriterWith7Bit(Stream input) : 
            base(input) { }

        public BinaryWriterWith7Bit(Stream input, Encoding encoding) :
            base(input, encoding) { }
        public BinaryWriterWith7Bit(Stream input, Encoding encoding, bool leaveOpen) :
            base(input, encoding, leaveOpen) { }

        public new void Write7BitEncodedInt(int value)
        {
            base.Write7BitEncodedInt(value);
        }
    }
}