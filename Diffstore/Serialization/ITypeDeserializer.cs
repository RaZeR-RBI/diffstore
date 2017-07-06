using System.IO;

namespace Diffstore.Serialization
{
    public interface ITypeDeserializer<T>
    {
         T Deserialize(Stream input);
    }
}