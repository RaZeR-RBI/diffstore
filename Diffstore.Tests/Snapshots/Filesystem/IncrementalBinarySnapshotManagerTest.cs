using System;
using Diffstore.Entities.Filesystem;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Snapshots.Filesystem
{
    public class IncrementalBinarySnapshotManagerTest : SnapshotManagerTest
    {
        protected override ISnapshotManager<long, SampleData> Build()
        {
            var options = new FilesystemStorageOptions();
            var filesystem = new MemoryFileSystem();
            return new IncrementalBinarySnapshotManager<long, SampleData>(
                options, filesystem, 64);
        }

        [Fact]
        public new void ShouldMakeSelectAndDropCorrectly()
        {
            base.ShouldMakeSelectAndDropCorrectly();
        }
    }
}