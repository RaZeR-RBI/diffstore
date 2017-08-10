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
            var options = new FilesystemStorageOptions()
            {
                MaxSnapshotFileSize = 512
            };
            var filesystem = new MemoryFileSystem();
            return new IncrementalBinarySnapshotManager<long, SampleData>(
                options, filesystem, 128);
        }
    }
}