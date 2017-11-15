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
        [MemberData("Builders", MemberType = 
            typeof(TestBuilderGenerator<SampleDataWithCollections>))]
        public void ShouldProvideBusinessLogic(
            Func<IDiffstore<long, SampleDataWithCollections>> builder)
        {
            // Configure
            var db = builder();

            // Create
            Assert.Empty(db.Keys);

            var key = 1L;
            var value = new SampleDataWithCollections()
            {
                IntProperty = 1337,
                PublicString = "Diffstore rules",
                MyList = new List<string> { "Apple", "Orange" },
                MyDictionary = new Dictionary<string, int> {
                    { "Apples", 5 },
                    { "Oranges", 3 }
                }
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

        [Theory]
        [MemberData("Builders", MemberType = typeof(TestBuilderGenerator<SampleData>))]
        public void ShouldRaiseEvents(Func<IDiffstore<long, SampleData>> builder)
        {
            // Arrange
            var db = builder();
            var value = new SampleData() { PublicString = "Hello" };
            var entity = Entity.Create(1L, value);
            int saveEventCount = 0; int deleteEventCount = 0;
            SetupAssertEvents(entity, db);
            db.OnSave += (s, e) => saveEventCount++;
            db.OnDelete += (s, e) => deleteEventCount++;

            // Act
            entity.SaveIn(db);
            var duplicateEntity = Entity.Create(1L, value);
            duplicateEntity.SaveIn(db);
            duplicateEntity.DeleteFrom(db);
            entity.DeleteFrom(db);

            // Assert
            Assert.Equal(1, saveEventCount); // must not fire on duplicate
            Assert.Equal(1, deleteEventCount); // same here
        }

        // Helper method
        private void SetupAssertEvents(Entity<long, SampleData> entity, 
            IDiffstore<long, SampleData> db)
        {
            db.OnSave += (sender, saved) => {
                Assert.Equal(db, sender);
                Assert.Equal(entity, saved);
            };
            db.OnDelete += (sender, key) => {
                Assert.Equal(db, sender);
                Assert.Equal(key, entity.Key);
            };
        }

        private static class TestBuilderGenerator<T>
            where T : class, new()
        {
            private static readonly List<Func<IDiffstore<long, T>>> _builders =
                new List<Func<IDiffstore<long, T>>>()
                {
                    BinaryStatisticsOptimized,
                    () => SingleFile(FileFormat.XML)
                };

            private static IDiffstore<long, T> BinaryStatisticsOptimized()
            {
                return new DiffstoreBuilder<long, T>()
                    .WithMemoryStorage()
                    .WithFileBasedEntities()
                    .WithLastFirstOptimizedSnapshots()
                    .Setup();
            }

            private static IDiffstore<long, T> SingleFile(FileFormat format)
            {
                return new DiffstoreBuilder<long, T>()
                    .WithMemoryStorage()
                    .WithFileBasedEntities() // TODO FIXME
                    .WithSingleFileSnapshots(format)
                    .Setup();
            }

            public static IEnumerable<object[]> Builders
            {
                get { return _builders.Select(x => new [] { (object)x }); }
            }
        }
    }
}