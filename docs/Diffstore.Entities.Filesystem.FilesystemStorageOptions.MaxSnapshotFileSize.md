# MaxSnapshotFileSize

**Field**

**Namespace:** [Diffstore.Entities.Filesystem](Diffstore.Entities.Filesystem.md)

**Declared in:** [Diffstore.Entities.Filesystem.FilesystemStorageOptions](Diffstore.Entities.Filesystem.FilesystemStorageOptions.md)

------



Specifies the maximum snapshot file size. Defaults to 1 MB.
Note: the file may contain one or more snapshots depending on
the used ISnapshotManager implementation.


## Syntax

```csharp
public long MaxSnapshotFileSize = 1 * 1024 * 1024;
```

------

[Back to index](index.md)