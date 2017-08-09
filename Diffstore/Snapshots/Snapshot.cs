using System;
using Diffstore.Entities;

namespace Diffstore.Snapshots
{
    public class Snapshot<TKey, TValue>
    {
        public long Time { get; }
        public Entity<TKey, TValue> State { get; }

        public Snapshot(long time, Entity<TKey, TValue> state) =>
            (Time, State) = (time, state);

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var p = obj as Snapshot<TKey, TValue>;
            if ((System.Object)p == null) return false;

            return (Time.Equals(p.Time)) && (State.Equals(p.State));
        }

        public override int GetHashCode()
        {
            return unchecked(Time.GetHashCode() + State.GetHashCode());
        }

        public override string ToString()
        {
            return $"T: {Time}, S: {State.ToString()}";
        }
    }

    public static class Snapshot
    {
        public static Snapshot<TK, TV> Create<TK, TV>(long time, Entity<TK, TV> entity)
        {
            return new Snapshot<TK, TV>(time, entity);
        }

        public static Snapshot<TK, TV> Create<TK, TV>(Entity<TK, TV> entity)
        {
            return Create(GetCurrentUnixSeconds(), entity);
        }

        public static long GetCurrentUnixSeconds()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }
    }
}