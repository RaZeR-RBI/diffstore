# DeleteFrom<TK, TV>(Entity<TK, TV>, IDiffstore<TK, TV>)

**Method**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore.EntityExtensions](Diffstore.EntityExtensions.md)

------



Deletes this entity from the specified storage.


## Syntax

```csharp
public static void DeleteFrom<TK, TV>(
	Entity<TK, TV> entity,
	IDiffstore<TK, TV> db
)
```

### Parameters

`db`

Storage in which the entity should be deleted.

------

[Back to index](index.md)