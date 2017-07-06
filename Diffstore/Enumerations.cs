namespace Diffstore
{
    public enum StorageType
    {
        FilesOnly, Cached, InMemory
    }

    public enum Partitioning
    {
        None, FileSize, Timespan
    }
}