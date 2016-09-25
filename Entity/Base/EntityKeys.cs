
using System;

namespace Entity
{
    public static class EntityKeys
    {

        #region Cache keys

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object</returns>
        public static string KeyDelete<T>(int objectID) where T: class
        {
            return ConstructKey(typeof(T), ActionType.Delete, objectID.ToString());
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="codeName">Code name of object</param>
        /// <returns>Cache key for deleting an object</returns>
        public static string KeyDeleteCodeName<T>(string codeName) where T : class
        {
            return ConstructKey(typeof(T), ActionType.Delete, codeName);
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public static string KeyDelete<T>(string objectID) where T : class
        {
            return ConstructKey(typeof(T), ActionType.Delete, objectID.ToString());
        }

        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object</returns>
        public static string KeyUpdate<T>(int objectID) where T : class
        {
            return ConstructKey(typeof(T), ActionType.Update, objectID.ToString());
        }

        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="codeName">Code name of object</param>
        /// <returns>Cache key for updating an object</returns>
        public static string KeyUpdateCodeName<T>(string codeName) where T : class
        {
            return ConstructKey(typeof(T), ActionType.Update, codeName);
        }

        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object</returns>
        public static string KeyUpdate<T>(string objectID) where T : class
        {
            return ConstructKey(typeof(T), ActionType.Update, objectID.ToString());
        }

        /// <summary>
        /// Gets cache key for update any object action
        /// </summary>
        /// <returns>Cache key for updating any object of given type</returns>
        public static string KeyUpdateAny<T>() where T : class
        {
            return ConstructKey(typeof(T), ActionType.UpdateAny);
        }

        /// <summary>
        /// Gets cache key for delete any object action
        /// </summary>
        /// <returns>Cache key for deleting any object of given type</returns>
        public static string KeyDeleteAny<T>() where T : class
        {
            return ConstructKey(typeof(T), ActionType.DeleteAny);
        }

        /// <summary>
        /// Gets cache key for creation of any object of given type
        /// </summary>
        /// <returns>Cache key for creating any object of given type</returns>
        public static string KeyCreateAny<T>() where T : class
        {
            return ConstructKey(typeof(T), ActionType.CreateAny);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Creates key from given action type
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="actionType">Action type</param>
        /// <returns></returns>
        private static string ConstructKey(Type type, ActionType actionType)
        {
            return string.Format("{0}.{1}", type.FullName, actionType.Value);
        }

        /// <summary>
        /// Creates key from given action type and object ID 
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="actionType">Action type</param>
        /// <param name="objectID">ObjectID if necessary</param>
        /// <returns></returns>
        private static string ConstructKey(Type type, ActionType actionType, string objectID)
        {
            return string.Format("{0}.{1}[{2}]", type.FullName, actionType.Value, objectID);
        }

        #endregion
    }
}
