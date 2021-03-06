using System;
using Diffstore.Entities.Filesystem;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Snapshots.Filesystem
{
    public class BinaryLIFOSnapshotManagerTest : SnapshotManagerTest
    {
        internal override ISnapshotManager<long, SampleData> Build()
        {
            var options = new FilesystemStorageOptions()
            {
                MaxSnapshotFileSize = 512
            };
            var filesystem = new MemoryFileSystem();
            return new BinaryLIFOSnapshotManager<long, SampleData>(
                options, filesystem, 128);
        }
    }
}