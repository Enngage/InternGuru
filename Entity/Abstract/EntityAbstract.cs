
using System;

namespace Entity
{
    public abstract class EntityAbstract
    {
        #region Cache keys

        /// <summary>
        /// Gets cache key for "create" object action
        /// </summary>
        /// <returns>Cache key for creating new object of current class</returns>
        public static String KeyCreate<T>() where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".create";
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public static String KeyDelete<T>() where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".delete";
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public static String KeyDelete<T>(int objectID) where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".delete." + objectID;
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public static String KeyDelete<T>(string objectID) where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".delete." + objectID;
        }

        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object of current class</returns>
        public static String KeyUpdate<T>(int objectID) where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".update." + objectID;
        }


        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object of current class</returns>
        public static String KeyUpdate<T>(string objectID) where T : EntityAbstract
        {
            Type type = typeof(T);
            return type.Name + ".update." + objectID;
        }

        #endregion
    }
}
