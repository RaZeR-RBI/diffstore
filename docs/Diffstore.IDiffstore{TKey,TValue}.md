# IDiffstore<TKey, TValue>

**Interface**

**Namespace:** [Diffstore](Diffstore.md)

**Declared in:** [Diffstore](Diffstore.md)

------



Defines all supported operations.


## Members

### Property
* [Keys](Diffstore.IDiffstore{TKey,TValue}.Keys.md)
* [Entities](Diffstore.IDiffstore{TKey,TValue}.Entities.md)
* [OnSave](Diffstore.IDiffstore{TKey,TValue}.OnSave.md)
* [OnDelete](Diffstore.IDiffstore{TKey,TValue}.OnDelete.md)

### Indexer
* [Entity<TKey, TValue>[TKey]](Diffstore.IDiffstore{TKey,TValue}.Entity{TKey,TValue}[TKey].md)

### Method
* [Delete(Entity<TKey, TValue>)](Diffstore.IDiffstore{TKey,TValue}.Delete(Entity{TKey,TValue}).md)
* [Delete(TKey)](Diffstore.IDiffstore{TKey,TValue}.Delete(TKey).md)
* [Exists(TKey)](Diffstore.IDiffstore{TKey,TValue}.Exists(TKey).md)
* [Get(TKey)](Diffstore.IDiffstore{TKey,TValue}.Get(TKey).md)
* [GetFirst(TKey)](Diffstore.IDiffstore{TKey,TValue}.GetFirst(TKey).md)
* [GetFirstTime(TKey)](Diffstore.IDiffstore{TKey,TValue}.GetFirstTime(TKey).md)
* [GetLast(TKey)](Diffstore.IDiffstore{TKey,TValue}.GetLast(TKey).md)
* [GetLastTime(TKey)](Diffstore.IDiffstore{TKey,TValue}.GetLastTime(TKey).md)
* [GetSnapshots(TKey, int, int)](Diffstore.IDiffstore{TKey,TValue}.GetSnapshots(TKey,int,int).md)
* [GetSnapshots(TKey)](Diffstore.IDiffstore{TKey,TValue}.GetSnapshots(TKey).md)
* [GetSnapshotsBetween(TKey, long, long)](Diffstore.IDiffstore{TKey,TValue}.GetSnapshotsBetween(TKey,long,long).md)
* [PutSnapshot(Entity<TKey, TValue>, long)](Diffstore.IDiffstore{TKey,TValue}.PutSnapshot(Entity{TKey,TValue},long).md)
* [Save(Entity<TKey, TValue>, bool)](Diffstore.IDiffstore{TKey,TValue}.Save(Entity{TKey,TValue},bool).md)
* [Save(TKey, TValue, bool)](Diffstore.IDiffstore{TKey,TValue}.Save(TKey,TValue,bool).md)

------

[Back to index](index.md)