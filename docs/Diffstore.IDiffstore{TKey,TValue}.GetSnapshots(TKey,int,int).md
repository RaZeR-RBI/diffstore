# GetSnapshots(TKey, int, int)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.IDiffstore<TKey, TValue>](Diffstore.IDiffstore{TKey,TValue}.md)

------



Fetches snapshots page for the specified entity key.


## Syntax

```csharp
public IEnumerable<Snapshot<TKey, TValue>> GetSnapshots(
	TKey key,
	int from,
	int count
)
```

------

[Back to index](index.md)