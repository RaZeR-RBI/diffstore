using System;
using System.Collections.Generic;
using System.Linq;
using Diffstore.Serialization;

namespace Diffstore.Entities
{
    public class EntityManager<TKey, TValue, TInput, TOutput> : IEntityManager<TKey, TValue>
        where TKey : IComparable
        where TValue : class, new()
        where TInput : IDisposable
        where TOutput : IDisposable
    {
        private readonly IFormatter<TInput, TOutput> formatter;
        private readonly IEntityReaderWriter<TKey, TInput, TOutput> io;
        private readonly Schema schema;

        public EntityManager(
            IFormatter<TInput, TOutput> formatter,
            IEntityReaderWriter<TKey, TInput, TOutput> entityIO)
            =>
            (this.formatter, this.io, schema) =
            (formatter, entityIO, SchemaManager.Get<TValue>());

        public void Delete(TKey key)
        {
            io.Drop(key);
        }

        public void Delete(Entity<TKey, TValue> entity)
        {
            Delete(entity.Key);
        }

        public bool Exists(TKey key)
        {
            return io.Exists(key);
        }

        public bool Exists(Entity<TKey, TValue> entity)
        {
            return Exists(entity.Key);
        }

        public Entity<TKey, TValue> Get(TKey key)
        {
            using (var stream = io.BeginRead(key)) return Entity.Create(key, Read(stream));
        }

        public IEnumerable<Entity<TKey, TValue>> GetAll()
        {
            return io.GetAllKeys().Select(key => Get(key));
        }

        public IEnumerable<TKey> GetKeys()
        {
            return io.GetAllKeys().AsEnumerable();
        }

        public void Persist(Entity<TKey, TValue> entity)
        {
            using (var stream = io.BeginWrite(entity.Key)) Write(stream, entity.Value);
        }

        public void Persist(TKey key, TValue value)
        {
            Persist(Entity.Create(key, value));
        }

        private TValue Read(TInput input)
        {
            var result = new TValue();
            foreach (var field in schema.Fields)
            {
                var value = formatter.Deserialize(field.Type, input, field.Name);
                if (value != null) field.Setter(result, value);
            }
            return result;
        }

        private void Write(TOutput output, TValue data)
        {
            foreach (var field in schema.Fields)
            {
                var value = field.Getter(data);
                formatter.Serialize(value, output, field.Name);
            }
        }
    }
}