using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Diffstore.Entities;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization;
using Moq;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Entities.EntityManager
{
    public class FilesystemBinaryEMTest : EntityManagerTest
    {
        public override void ShouldFetchSpecifiedCountIfLazy(long[] keys)
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

        protected override IEntityManager<long, SampleData> Build()
        {
            var filesystem = new MemoryFileSystem();
            var formatter = FastBinaryFormatter.Instance;
            var options = new FilesystemStorageOptions()
            {
                EntitiesPerDirectory = 1000
            };

            var entityIO = new FilesystemEntityReaderWriter<long, BinaryReader, BinaryWriter>
                (filesystem, formatter, options);

            return new EntityManager<long, SampleData, BinaryReader, BinaryWriter>
                (formatter, entityIO);
        }
    }
}