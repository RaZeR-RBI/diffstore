using System;
using System.IO;
using Diffstore.Snapshots;

namespace Diffstore.Tests
{
    public class SampleData
    {
        public string PublicString;
        public int IntProperty { get; set; }
        private string mySecret;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as SampleData;
            if ((System.Object)p == null) return false;

            return (PublicString == p.PublicString) &&
                    (IntProperty == p.IntProperty);
        }

        public override int GetHashCode()
        {
            return unchecked(PublicString.GetHashCode() + IntProperty);
        }
    }

    public class SampleDataInvalid : SampleData
    {
        public long ReadOnlyLong { get; }
    }

    public class SampleDataWithIgnoredFields : SampleData
    {
        [IgnoreChanges]
        public bool ignoreMe;

        [IgnoreChanges]
        public short IgnoreMeToo { get; set; }
    }
}