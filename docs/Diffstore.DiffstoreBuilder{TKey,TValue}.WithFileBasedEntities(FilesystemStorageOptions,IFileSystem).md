# WithFileBasedEntities(FilesystemStorageOptions, IFileSystem)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.DiffstoreBuilder<TKey, TValue>](Diffstore.DiffstoreBuilder{TKey,TValue}.md)

------



Sets up the engine to use file-based entity storage.


## Syntax

```csharp
public DiffstoreBuilder<TKey, TValue> WithFileBasedEntities(
	FilesystemStorageOptions options,
	IFileSystem fileSystem
)
```

### Parameters

`options`


If not null, overrides the default [FilesystemStorageOptions](Diffstore.Entities.Filesystem.FilesystemStorageOptions.md).


`fileSystem`


If not null, uses the specified IFileSystem, otherwise uses
what has been called before.


------

[Back to index](index.md)