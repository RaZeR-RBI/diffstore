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

        private static long GetCurrentUnixSeconds()
        {
            return ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
        }
    }
}