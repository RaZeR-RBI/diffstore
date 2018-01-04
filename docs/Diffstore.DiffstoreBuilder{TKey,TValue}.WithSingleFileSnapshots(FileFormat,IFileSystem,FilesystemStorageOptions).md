# WithSingleFileSnapshots(FileFormat, IFileSystem, FilesystemStorageOptions)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.DiffstoreBuilder<TKey, TValue>](Diffstore.DiffstoreBuilder{TKey,TValue}.md)

------



Sets up the engine to use the single-file-per-snapshot storage.
Suitable for big entities and/or when they need to be human-readable.


## Syntax

```csharp
public DiffstoreBuilder<TKey, TValue> WithSingleFileSnapshots(
	FileFormat format,
	IFileSystem fileSystem,
	FilesystemStorageOptions options
)
```

### Parameters

`format`


Specifies the [FileFormat](Diffstore.FileFormat.md) which will be used for
serialization and deserialization.


`options`


If not null, overrides the default [FilesystemStorageOptions](Diffstore.Entities.Filesystem.FilesystemStorageOptions.md).


`fileSystem`


If not null, uses the specified IFileSystem, otherwise uses
what has been called before.


------

[Back to index](index.md)