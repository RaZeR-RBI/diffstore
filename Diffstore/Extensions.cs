using System;
using Diffstore.Entities;

namespace Diffstore
{
    /// <summary>
    /// Defines several extension methods to use with <see cref="Entity<TKey, TValue>"/>.
    /// </summary>
    public static class EntityExtensions
    {
        /// <summary>
        /// Saves this entity in the specified storage.
        /// </summary>
        /// <param name="db">Storage in which the entity should be saved.</param>
        /// <param name="makeSnapshot">If true, creates a snapshot if entity was changed.</param>
        public static void SaveIn<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db,
            bool makeSnapshot = true)
            where TK : IComparable
            where TV : class, new() =>
            db.Save(entity, makeSnapshot);

        /// <summary>
        /// Deletes this entity from the specified storage.
        /// </summary>
        /// <param name="db">Storage in which the entity should be deleted.</param>
        public static void DeleteFrom<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db)
            where TK : IComparable
            where TV : class, new() =>
            db.Delete(entity);

        /// <summary>
        /// Checks if this entity exists in the specified storage.
        /// </summary>
        /// <param name="db">Storage to be checked.</param>
        public static bool ExistsIn<TK, TV>(this Entity<TK, TV> entity, IDiffstore<TK, TV> db)
            where TK : IComparable
            where TV : class, new() =>
            db.Exists(entity.Key);
    }
}