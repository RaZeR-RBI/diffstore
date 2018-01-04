# WithDiskStorage(string)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.DiffstoreBuilder<TKey, TValue>](Diffstore.DiffstoreBuilder{TKey,TValue}.md)

------



Sets up the engine to use disk storage.


## Syntax

```csharp
public DiffstoreBuilder<TKey, TValue> WithDiskStorage(
	string subfolder
)
```

### Parameters

`subfolder`


Subfolder for files relative to the app location.
Defaults to "storage".


------

[Back to index](index.md)