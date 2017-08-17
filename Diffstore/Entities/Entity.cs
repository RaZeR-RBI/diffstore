using System;
using Diffstore.Serialization;

namespace Diffstore.Entities
{
    public class Entity<TKey, TValue>
    {
        public TKey Key { get; set; }
        public TValue Value { get; set; }

        public Entity(TKey key, TValue value) => (Key, Value) = (key, value);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as Entity<TKey, TValue>;
            if ((System.Object)p == null) return false;

            return (Key.Equals(p.Key)) && (Value.Equals(p.Value));
        }

        public override int GetHashCode()
        {
            return unchecked(Key.GetHashCode() + Value.GetHashCode());
        }

        public override string ToString()
        {
            return $"[{Key} => {Value}]";
        }
    }

    public static class Entity
    {
        public static Entity<TK, TV> Create<TK, TV>(TK key, TV value)
        {
            return new Entity<TK, TV>(key, value);
        }
    }
}