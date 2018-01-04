using System;
using System.Xml;
using Diffstore.Entities.Filesystem;
using Diffstore.Serialization.XML;
using Diffstore.Snapshots;
using Diffstore.Snapshots.Filesystem;
using SharpFileSystem.FileSystems;
using Xunit;

namespace Diffstore.Tests.Snapshots.Filesystem
{
    public class XmlSingleFileSnapshotManagerTest : SnapshotManagerTest
    {
        internal override ISnapshotManager<long, SampleData> Build()
        {
            var options = new FilesystemStorageOptions();
            var filesystem = new MemoryFileSystem();
            return new SingleFileSnapshotManager<long, SampleData, XmlDocumentAdapter, XmlWriterAdapter>(
                options, new XmlFormatter(), filesystem);
        }
    }
}