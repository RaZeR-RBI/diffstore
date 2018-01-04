# GetSnapshotsBetween(TKey, long, long)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.IDiffstore<TKey, TValue>](Diffstore.IDiffstore{TKey,TValue}.md)

------



Fetches snapshots in time range [timeStart, timeEnd).


## Syntax

```csharp
public IEnumerable<Snapshot<TKey, TValue>> GetSnapshotsBetween(
	TKey key,
	long timeStart,
	long timeEnd
)
```

------

[Back to index](index.md)