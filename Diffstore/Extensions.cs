using System;
using Diffstore.Entities;

namespace Diffstore
{
    public static class EntityExtensions
    {
        public static void SaveIn<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db,
            bool makeSnapshot = true)
            where TK : IComparable
            where TV : class, new()
        {
            db.Save(entity, makeSnapshot);
        }

        public static void DeleteFrom<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db)
            where TK : IComparable
            where TV : class, new()
        {
            db.Delete(entity);
        }

        public static bool ExistsIn<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db)
            where TK : IComparable
            where TV : class, new()
        {
            return db.Exists(entity.Key);
        }
    }
}