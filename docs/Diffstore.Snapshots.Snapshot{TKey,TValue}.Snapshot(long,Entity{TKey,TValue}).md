# Snapshot(long, Entity<TKey, TValue>)

**Constructor**

**Namespace:** [Diffstore.Snapshots](Diffstore.Snapshots.md)

**Declared in:** [Diffstore.Snapshots.Snapshot<TKey, TValue>](Diffstore.Snapshots.Snapshot{TKey,TValue}.md)

------



Creates a snapshot of a given entity marked with specified time.


## Syntax

```csharp
public Snapshot(
	long time,
	Entity<TKey, TValue> state
)
```

### Parameters

`time`

Snapshot creation time.

`state`

Entity value at the specified moment.

------

[Back to index](index.md)