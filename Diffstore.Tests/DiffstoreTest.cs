using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Diffstore.Entities;
using Xunit;

namespace Diffstore.Tests
{
    public class DiffstoreTest
    {
        [Theory]
        [MemberData("Builders", MemberType = typeof(TestBuilderGenerator))]
        public void ShouldProvideBusinessLogic(Func<IDiffstore<long, SampleData>> builder)
        {
            // Configure
            var db = builder();

            // Create
            Assert.Empty(db.Keys);

            var key = 1L;
            var value = new SampleData()
            {
                IntProperty = 1337,
                PublicString = "Diffstore rules"
            };
            var entity = Entity.Create(key, value);

            Assert.False(entity.ExistsIn(db));
            entity.SaveIn(db);
            Assert.True(entity.ExistsIn(db));

            // Read
            var expectedEntities = new[] { entity }.AsEnumerable();
            var actualEntities = db.Entities;

            Assert.Equal(expectedEntities, actualEntities);
            Assert.Equal(key, db[key].Key);
            Assert.Equal(value, db[key].Value);

            // Update
            value.PublicString = "Check me";
            entity.SaveIn(db);

            Assert.Equal("Check me", db[key].Value.PublicString);

            // Delete
            entity.DeleteFrom(db);
            Assert.False(entity.ExistsIn(db));
        }

        private static class TestBuilderGenerator
        {
            private static readonly List<Func<IDiffstore<long, SampleData>>> _builders =
                new List<Func<IDiffstore<long, SampleData>>>()
                {
                    BinaryStatisticsOptimized
                };

            private static IDiffstore<long, SampleData> BinaryStatisticsOptimized()
            {
                return new DiffstoreBuilder<long, SampleData>()
                    .WithMemoryStorage()
                    .WithFileBasedEntities()
                    .WithLastFirstOptimizedSnapshots()
                    .Setup();
            }

            public static IEnumerable<object[]> Builders
            {
                get { return _builders.Select(x => new [] { (object)x }); }
            }
        }
    }
}