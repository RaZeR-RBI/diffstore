using System;

namespace Diffstore.Serialization
{
    public interface ISerializer<TInputStream, TOutputStream>
    {
         void Serialize(object value, TOutputStream stream);
         object Deserialize(Type type, TInputStream stream);
    }
}