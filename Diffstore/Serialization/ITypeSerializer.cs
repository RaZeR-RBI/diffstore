using System.IO;

namespace Diffstore.Serialization
{
    public interface ITypeSerializer<T>
    {
         void Serialize(T input, Stream stream);
    }
}