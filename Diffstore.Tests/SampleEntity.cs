using System;
using System.IO;

namespace Diffstore.Tests
{
    public class SampleEntity
    {
        public string PublicString;
        public int IntProperty { get; set; }
        private string mySecret;

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as SampleEntity;
            if ((System.Object)p == null) return false;

            return (PublicString == p.PublicString) &&
                    (IntProperty == p.IntProperty);
        }

        public override int GetHashCode()
        {
            return unchecked(PublicString.GetHashCode() + IntProperty);
        }
    }

    public class SampleEntityInvalid : SampleEntity
    {
        public long ReadOnlyLong { get; }
    }
}