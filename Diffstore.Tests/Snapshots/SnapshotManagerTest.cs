using System.Collections.Generic;
using System.Linq;
using Diffstore.Entities;
using Diffstore.Snapshots;
using Xunit;

namespace Diffstore.Tests.Snapshots
{
    public abstract class SnapshotManagerTest
    {
        internal abstract ISnapshotManager<long, SampleData> Build();
        protected const long KEY = 1L;

        [Fact]
        public void ShouldMakeSelectAndDropCorrectly()
        {
            var sm = Build();
            var times = Enumerable.Range(0, 50);
            var expectedSnapshots = new List<Snapshot<long, SampleData>>();
            
            Assert.False(sm.Any(KEY));
            foreach(var time in times)
            {
                var entity = MakeEntity(KEY, time + 0b0111011101110000);
                var expected = MakeSnapshot(time, entity);
                expectedSnapshots.Add(expected);
                sm.Make(entity, time);
            }
            Assert.True(sm.Any(KEY));

            // GetAll
            var allSnapshots = sm.GetAll(KEY).OrderBy(i => i.Time);
            Assert.Equal(expectedSnapshots, allSnapshots);

            // GetFirst, GetLast
            Assert.Equal(times.First(), sm.GetFirstTime(KEY));
            Assert.Equal(times.Last(), sm.GetLastTime(KEY));
            Assert.Equal(expectedSnapshots.First(), sm.GetFirst(KEY));
            Assert.Equal(expectedSnapshots.Last(), sm.GetLast(KEY));

            // GetPage, GetInRange
            var expectedPageAsc = expectedSnapshots.Skip(1).Take(3);
            var actualPageAsc = sm.GetPage(KEY, 1, 3, true);
            Assert.Equal(expectedPageAsc, actualPageAsc);

            var expectedPageDesc = allSnapshots.Reverse().Skip(1).Take(3);
            var actualPageDesc = sm.GetPage(KEY, 1, 3, false);
            Assert.Equal(expectedPageDesc, actualPageDesc);

            var expectedRange = expectedSnapshots.Where(x => x.Time >= 3 && x.Time < 10);
            var actualRange = sm.GetInRange(KEY, 3, 10).OrderBy(i => i.Time);
            Assert.Equal(expectedRange, actualRange);

            // Drop
            sm.Drop(KEY);
            Assert.Empty(sm.GetAll(KEY));
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