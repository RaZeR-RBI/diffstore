using System;
using System.Linq;
using System.Reflection;
using Diffstore.Serialization;
using Xunit;

namespace Diffstore.Tests.Serialization
{
    public class SchemaTest
    {
        [Fact]
        public void ShouldFindPublicFields()
        {
            var schema = new Schema(typeof(SampleData));
            Assert.Equal(2, schema.Fields.Count);
        }

        [Fact]
        public void ShouldFindPrivateFieldsIfSpecified()
        {
            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var schema = new Schema(typeof(SampleData), flags);
            Assert.Equal(4, schema.Fields.Count); // include backing field for the int property
        }

        [Fact]
        public void ShouldFailOnMissingAccessor()
        {
            var ex = Assert.Throws<MissingFieldException>(() => new Schema(typeof(SampleDataInvalid)));
            Assert.True(ex.Message.Contains("ReadOnlyLong"));
        }

        [Fact]
        public void ShouldMarkIgnoredFields()
        {
            var schema = new Schema(typeof(SampleDataWithIgnoredFields));
            var actual = schema.Fields
                .Where((field) => field.IgnoreChanges)
                .Count();

            Assert.Equal(2, actual);
        }
    }
}