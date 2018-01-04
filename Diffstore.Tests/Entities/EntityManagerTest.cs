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

namespace Diffstore.Tests.Entities
{
    public abstract class EntityManagerTest
    {
        internal abstract IEntityManager<long, SampleData> Build();

        [Theory]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(1337)]
        public void ShouldProvideCRUD(long key)
        {
            var em = Build();
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
        public abstract void ShouldFetchSpecifiedCountIfLazy(long[] keys);

        [Fact]
        public void ShouldWorkWithNullValues()
        {
            var em = Build();
            var expected = new SampleData();
            em.Persist(10, expected);
            var actual = em.Get(10).Value;
            Assert.Equal(expected, actual);
        }
    }
}