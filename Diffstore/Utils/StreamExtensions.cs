using System;
using System.IO;

namespace Diffstore.Utils
{
    /// <summary>
    /// Defines stream-related extension methods.
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads all bytes from the specified stream using an intermediate buffer.
        /// </summary>
        public static byte[] ReadAllBytes(this Stream reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                reader.Close();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Reads specified count of bytes from the specified stream using an intermediate buffer.
        /// </summary>
        public static MemoryStream ReadBytes(this Stream stream, int count,
            bool close = false)
        {
            byte[] buffer = new byte[count];
            stream.Read(buffer, 0, count);
            if (close) stream.Close();
            return new MemoryStream(buffer);
        }
    }
}