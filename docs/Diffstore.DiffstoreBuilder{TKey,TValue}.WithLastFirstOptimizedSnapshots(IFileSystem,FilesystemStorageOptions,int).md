# WithLastFirstOptimizedSnapshots(IFileSystem, FilesystemStorageOptions, int)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.DiffstoreBuilder<TKey, TValue>](Diffstore.DiffstoreBuilder{TKey,TValue}.md)

------



Sets up the engine to use the last-in-first-out optimized snapshot storage,
suitable for fast reading of recent data.


## Syntax

```csharp
public DiffstoreBuilder<TKey, TValue> WithLastFirstOptimizedSnapshots(
	IFileSystem fileSystem,
	FilesystemStorageOptions options,
	int zeroPaddingBytes
)
```

### Parameters

`formatter`


Specifies a [IFormatter<TInputStream, TOutputStream>](Diffstore.Serialization.IFormatter{TInputStream,TOutputStream}.md) which
will be used for serialization and deserialization.


`options`


If not null, overrides the default [FilesystemStorageOptions](Diffstore.Entities.Filesystem.FilesystemStorageOptions.md).


`zeroPaddingBytes`


Specifies maximum free space at the start of the file for future
snapshots. When there is no free space left for writing, the file
expands by specified byte count and rewrites.


If the number is too small, rewrites may appear often.



If the number is big, files will be bigger and may consist
of big unused space depending on how many data is being written.



------

[Back to index](index.md)