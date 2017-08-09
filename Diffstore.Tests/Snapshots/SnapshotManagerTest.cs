using System.Collections.Generic;
using System.Linq;
using Diffstore.Entities;
using Diffstore.Snapshots;
using Xunit;

namespace Diffstore.Tests.Snapshots
{
    public abstract class SnapshotManagerTest
    {
        protected abstract ISnapshotManager<long, SampleData> Build();
        protected const long KEY = 1L;

        [Fact]
        public void ShouldSelectCorrectly()
        {
            var sm = Build();
            var times = new int[] { 0, 1, 2, 3, 4, 10 }.AsEnumerable();
            var expectedSnapshots = new List<Snapshot<long, SampleData>>();
            
            foreach(var time in times)
            {
                var entity = MakeEntity(KEY, time);
                var expected = MakeSnapshot(time, entity);
                expectedSnapshots.Add(expected);
                Assert.True(sm.Make(entity, time, true));
            }

            // GetAll
            var allSnapshots = sm.GetAll(KEY);
            Assert.Equal(expectedSnapshots, allSnapshots);

            // GetFirst, GetLast
            Assert.Equal(times.First(), sm.GetFirstTime(KEY));
            Assert.Equal(times.Last(), sm.GetLastTime(KEY));
            Assert.Equal(expectedSnapshots.First(), sm.GetFirst(KEY));
            Assert.Equal(expectedSnapshots.Last(), sm.GetLast(KEY));

            // GetPage, GetInRange
            var expectedPage = expectedSnapshots.Skip(1).Take(3);
            var actualPage = sm.GetPage(KEY, 1, 3);
            Assert.Equal(expectedPage, actualPage);

            var expectedRange = expectedSnapshots.Where(x => x.Time >= 3 && x.Time < 10);
            var actualRange = sm.GetInRange(KEY, 3, 10);
            Assert.Equal(expectedRange, actualRange);

            // Drop
            sm.Drop(KEY);
            Assert.Empty(sm.GetAll(KEY));
        }

        [Fact]
        public void ShouldPreventDuplicatesIfSpecified()
        {
            var sm = Build();
            var entity = MakeEntity(KEY, 1);
            var unchangedEntity = MakeEntity(KEY, 1);

            Assert.True(sm.Make(entity, true));
            Assert.Single(sm.GetAll(KEY));
            Assert.False(sm.Make(unchangedEntity, true));
            Assert.Single(sm.GetAll(KEY));
        }

        protected Entity<long, SampleData> MakeEntity(long key, int data)
        {
            return Entity.Create(KEY, new SampleData() {
                IntProperty = data
            });
        }

        protected Snapshot<long, SampleData> MakeSnapshot(long time, Entity<long, SampleData> entity) {
            return Snapshot.Create<long, SampleData>(time, entity);
        }
    }
}