using System;

namespace Diffstore.Serialization
{
    /// <summary>
    /// Represents a formatter which is used for the file I/O
    /// </summary>
    public interface IFormatter<TInputStream, TOutputStream>
    {
        /// <summary>
        /// Serializes a value to the output stream
        /// </summary>
        /// <param name="value">The value to be serialized</param>
        /// <param name="stream">Output stream</param>
        void Serialize(object value, TOutputStream stream);

        /// <summary>
        /// Deserializes a value from the input stream
        /// </summary>
        /// <param name="type">Type of the value to be deserialized</param>
        /// <param name="stream">Input stream</param>
        /// <returns></returns>
        object Deserialize(Type type, TInputStream stream);
    }
}