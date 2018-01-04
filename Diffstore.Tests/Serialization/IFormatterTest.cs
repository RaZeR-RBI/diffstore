using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using Diffstore.Serialization;
using Diffstore.Serialization.XML;
using Diffstore.Utils;
using SharpFileSystem;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public abstract class IFormatterTest
    {
        internal abstract void BeginWrite(Stream input);
        internal abstract void EndWrite();
        internal abstract void BeginRead(Stream input);
        internal abstract void EndRead();

        internal abstract void Serialize(object val, string fieldName);
        internal abstract object Deserialize(Type type, string fieldName);

        private static readonly ReadOnlyCollection<object> Values = new List<object>()
        {
            /* Numbers */
            (byte)123,
            (short)12345,
            (int)1234567890,
            (long)123456789012345,
            (float)3.14,
            (double)3.14159265,
            (decimal)1000000.1,
            /* Strings and chars */
            'a',
            "Lorem ipsum",
            /* Boolean */
            true,
            false,
            /* Collections */
            new int[] { 1, 2, 3 },
            new List<int>() { 1, 2, 3 },
            new Dictionary<string, int>() {
                { "Apples", 2 },
                { "Oranges", 3 }
            }
        }.AsReadOnly();

        private static string Field(int index) => "Item" + index;

        public virtual void Test()
        {
            var fs = new MemoryFileSystem();
            var path = new FileSystemPath().AppendFile("test");

            // Write
            using (var outStream = fs.CreateFile(path))
            {
                BeginWrite(outStream);
                int i = 0;
                foreach (var value in Values)
                    Serialize(value, Field(i++));
                EndWrite();
            }

            // Read
            using (var inStream = fs.OpenFile(path, FileAccess.Read))
            {
                BeginRead(inStream);
                int i = 0;
                foreach (var expected in Values)
                {
                    var actual = Deserialize(expected.GetType(), Field(i++));
                    Assert.Equal(expected, actual);
                }
                EndRead();
            }
        }
    }
}