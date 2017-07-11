using System;
using System.Reflection;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class SchemaTest
    {
        private class TestData
        {
            public string PublicString;
            public int IntProperty { get; set; }
            private string mySecret;
        }

        private class TestDataInvalid : TestData
        {
            public long ReadOnlyLong { get; }
        }

        [Fact]
        public void FindingPublicFields()
        {
            var schema = new Schema(typeof(TestData));
            Assert.Equal(2, schema.Fields.Count);
        }

        [Fact]
        public void FindingPrivateFields()
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var schema = new Schema(typeof(TestData), flags);
            Assert.Equal(4, schema.Fields.Count); // include backing field for the int property
        }

        [Fact]
        public void FailOnMissingAccessor()
        {
            var ex = Assert.Throws<MissingFieldException>(() => new Schema(typeof(TestDataInvalid)));
            Assert.True(ex.Message.Contains("ReadOnlyLong"));
        }
    }
}