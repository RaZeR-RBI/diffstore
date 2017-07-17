using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Diffstore.Entities;
using Diffstore.IO;
using Diffstore.IO.Filesystem;
using Diffstore.Serialization;
using Moq;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Entities
{
    public class EntityManagerTest
    {
        private IEntityManager<long, SampleData> em;

        public EntityManagerTest()
        {
            var filesystem = new MemoryFileSystem();
            var formatter = FastBinaryFormatter.Instance;
            var options = new FilesystemEntityStorageOptions()
            {
                EntitiesPerDirectory = 1000
            };

            var entityIO = new FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter>
                (filesystem, formatter, options);

            em = new EntityManager<long, SampleData, BinaryReader, BinaryWriter>
                (formatter, entityIO);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(1337)]
        public void ShouldProvideCRUD(long key)
        {
            Assert.False(em.Exists(key));

            // Create
            var data = new SampleData()
            {
                IntProperty = 1,
                PublicString = "created"
            };
            em.Persist(key, data);
            Assert.True(em.Exists(key));

            // Read
            var actual = em.Get(key);
            Assert.Equal(data, actual.Value);

            // Update
            actual.Value.PublicString = "updated";
            actual.Value.IntProperty = 2;
            em.Persist(actual);

            var updated = em.Get(key);
            Assert.Equal(actual, updated);

            // Delete
            Assert.True(em.Exists(key));
            em.Delete(key);
            Assert.False(em.Exists(key));
        }

        [Theory]
        [InlineData(new long[] { 1, 2, 3, 4 })]
        public void ShouldFetchSpecifiedCountIfLazy(long[] keys)
        {
            var mockFormatter = new Mock<IFormatter<BinaryReader, BinaryWriter>>();
            mockFormatter.Setup((f) => f.Deserialize(
                It.IsAny<Type>(),
                It.IsAny<BinaryReader>())).Returns(null);

            var mockIO = new Mock<IEntityReaderWriter<long, BinaryReader, BinaryWriter>>();
            mockIO.Setup((io) => io.GetAllKeys()).Returns(keys);

            var em = new EntityManager<long, SampleData, BinaryReader, BinaryWriter>(
                mockFormatter.Object, mockIO.Object
            );
            var fetched = em.GetLazy(Comparer<long>.Default)
                            .Take(2);

            var count = fetched.Count();
            Assert.Equal(2, count);
            mockIO.Verify((io) => io.GetAllKeys(), Times.Once());
            mockIO.Verify((io) => io.BeginRead(It.IsAny<long>()), Times.Exactly(2));
        }

        [Fact]
        public void ShouldWorkWithNullValues()
        {
            var expected = new SampleData();
            em.Persist(10, expected);
            var actual = em.Get(10).Value;
            Assert.Equal(expected, actual);
        }
    }
}