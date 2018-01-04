# DiffstoreBuilder<TKey, TValue>

**Class**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore](Diffstore.md)

------



The main starting point, used to create a [IDiffstore<TKey, TValue>](Diffstore.IDiffstore{TKey,TValue}.md) instance.


### Type parameters

`TKey`


The type used as entity key.
All numeric types and strings are supported.
Any IComparable with a corresponding string representation should work.


`TValue`


The entity value, a class which contains the data that need to be stored.
Must be a class with a parameterless constructor.


## Members

### Method
* [Setup()](Diffstore.DiffstoreBuilder{TKey,TValue}.Setup().md)
* [WithDiskStorage(string)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithDiskStorage(string).md)
* [WithFileBasedEntities(FileFormat, FilesystemStorageOptions, IFileSystem)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithFileBasedEntities(FileFormat,FilesystemStorageOptions,IFileSystem).md)
* [WithFileBasedEntities(FilesystemStorageOptions, IFileSystem)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithFileBasedEntities(FilesystemStorageOptions,IFileSystem).md)
* [WithFileBasedEntities<TIn, TOut>(IFormatter<TIn, TOut>, FilesystemStorageOptions, IFileSystem)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithFileBasedEntities{TIn,TOut}(IFormatter{TIn,TOut},FilesystemStorageOptions,IFileSystem).md)
* [WithLastFirstOptimizedSnapshots(IFileSystem, FilesystemStorageOptions, int)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithLastFirstOptimizedSnapshots(IFileSystem,FilesystemStorageOptions,int).md)
* [WithMemoryStorage()](Diffstore.DiffstoreBuilder{TKey,TValue}.WithMemoryStorage().md)
* [WithSingleFileSnapshots(FileFormat, IFileSystem, FilesystemStorageOptions)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithSingleFileSnapshots(FileFormat,IFileSystem,FilesystemStorageOptions).md)

## Remarks

This API may change in the near future to allow function calls in any order.

## Examples


To create a new instance, first, choose a storage option - it's either
[WithMemoryStorage()](Diffstore.DiffstoreBuilder{TKey,TValue}.WithMemoryStorage().md) or [WithDiskStorage(string)](Diffstore.DiffstoreBuilder{TKey,TValue}.WithDiskStorage(string).md):

```csharp

var db = new DiffstoreBuilder<long, MyData>()
.WithDiskStorage();

```


Then you should use the type of storage for entities and snapshots.
At this time all entities are file-based, but in the future they may be stored
in relational databases or something other. Available formats are defined in
[FileFormat](Diffstore.FileFormat.md).



There are several mechanisms snapshot storage - currently implemented are
"single file per snapshot" (see [FileFormat](Diffstore.FileFormat.md)) and
"last in, first out" (optimized binary storage).



For example, to store your entities and snapshots in JSON files:


```csharp

var db = new DiffstoreBuilder<long, MyData>()
.WithDiskStorage()
.WithFileBasedEntities(FileFormat.JSON)
.WithSingleFileSnapshots(FileFormat.JSON);
.Setup();

```

And that's it! Now you can use the storage as you like. Check out the
[IDiffstore<TKey, TValue>](Diffstore.IDiffstore{TKey,TValue}.md) interface to see what can you do.


## See also
* [IDiffstore<TKey, TValue>](Diffstore.IDiffstore{TKey,TValue}.md)
* [FileFormat](Diffstore.FileFormat.md)

------

[Back to index](index.md)