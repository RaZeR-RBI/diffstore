using System;
using System.Collections.Generic;

namespace Diffstore.Entities
{
    internal interface IEntityManager<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
    {
        Entity<TKey, TValue> Get(TKey key);
        IEnumerable<Entity<TKey, TValue>> GetAll();
        IEnumerable<TKey> GetKeys();
        void Persist(TKey key, TValue value);
        void Persist(Entity<TKey, TValue> entity);
        void Delete(TKey key);
        void Delete(Entity<TKey, TValue> entity);
        bool Exists(TKey key);
        bool Exists(Entity<TKey, TValue> entity);
    }
}