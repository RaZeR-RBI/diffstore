using System;
using System.IO;

namespace Diffstore.Utils
{
    public static class Extensions
    {
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
    }
}