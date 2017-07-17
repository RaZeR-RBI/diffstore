using System.IO;
using Diffstore.Entities;
using Diffstore.IO.Filesystem;
using Diffstore.Serialization;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Entities
{
    public class EntityManagerTest
    {
        private IEntityManager<long, SampleEntity> em;

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

            em = new EntityManager<long, SampleEntity, BinaryReader, BinaryWriter>
                (formatter, entityIO);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(1337)]
        public void TestEntityLifecycle(long key)
        {
            Assert.False(em.Exists(key));

            // Create
            var data = new SampleEntity()
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
    }
}