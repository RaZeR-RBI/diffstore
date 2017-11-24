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
    public abstract class IFormatterTest<TIn, TOut>
        where TIn : IDisposable
        where TOut : IDisposable
    {
        protected abstract IFormatter<TIn, TOut> Build();

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
            new List<int>() { 1, 2, 3 },
            new Dictionary<string, int>() {
                { "Apples", 2 },
                { "Oranges", 3 }
            }
        }.AsReadOnly();

        private static string Field(int index) => "Item" + index;

        public virtual void Test()
        {
            var formatter = Build();
            var fs = new MemoryFileSystem();
            var path = new FileSystemPath().AppendFile("test");

            // Write
            using (var outStream = fs.CreateFile(path))
            using (var writer = StreamBuilder.FromStream<TOut>(outStream))
            {
                int i = 0;
                foreach (var value in Values)
                    formatter.Serialize(value, writer, Field(i++));
            }

            // Read
            using (var inStream = fs.OpenFile(path, FileAccess.Read))
            using (var reader = StreamBuilder.FromStream<TIn>(inStream))
            {
                int i = 0;
                foreach (var expected in Values)
                {
                    var actual = formatter.Deserialize(expected.GetType(), reader,
                        Field(i++));
                    Assert.Equal(expected, actual);
                }
            }
        }
    }
}