# Create<TK, TV>(long, Entity<TK, TV>)

**Method**

**Namespace:** [Diffstore.Snapshots](Diffstore.Snapshots.md)

**Declared in:** [Diffstore.Snapshots.Snapshot](Diffstore.Snapshots.Snapshot.md)

------



Creates a snapshot of a given entity marked with specified time.


## Syntax

```csharp
public static Snapshot<TK, TV> Create<TK, TV>(
	long time,
	Entity<TK, TV> entity
)
```

### Parameters

`time`

Snapshot creation time.

`state`

Entity value at the specified moment.

------

[Back to index](index.md)