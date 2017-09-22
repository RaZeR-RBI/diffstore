using System;
using System.IO;
using Diffstore.Entities;
using Diffstore.Snapshots;

namespace Diffstore.Tests
{
    public class SampleData
    {
        public string PublicString;
        public int IntProperty { get; set; }
        
        #pragma warning disable CS0169
        private string mySecret;
        #pragma warning restore CS0169

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

        public override string ToString()
        {
            return $"'{IntProperty}, {PublicString}'";
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

    public class SampleDataWithNonPersistableFields : SampleData
    {
        [DoNotPersist]
        public string IWillNotBeSaved;

        [DoNotPersist]
        public int IWillNotBeSavedToo { get; set; }
    }
}