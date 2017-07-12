namespace Diffstore.IO.Filesystem
{
    public class FilesystemTableStorageOptions
    {
        public enum Partitioning
        {
            None, RowCount
        }

        public Partitioning PartitionMode { get; set; }
        public long PartitionStep { get; set; }
    }
}