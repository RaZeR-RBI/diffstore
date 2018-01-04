# SaveIn<TK, TV>(Entity<TK, TV>, IDiffstore<TK, TV>, bool)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.EntityExtensions](Diffstore.EntityExtensions.md)

------



Saves this entity in the specified storage.


## Syntax

```csharp
public static void SaveIn<TK, TV>(
	Entity<TK, TV> entity,
	IDiffstore<TK, TV> db,
	bool makeSnapshot
)
```

### Parameters

`db`

Storage in which the entity should be saved.

`makeSnapshot`

If true, creates a snapshot if entity was changed.

------

[Back to index](index.md)