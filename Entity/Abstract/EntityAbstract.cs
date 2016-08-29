
namespace Entity
{
    public abstract class EntityAbstract
    {

        #region Cache keys

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public string KeyDelete(int objectID)
        {
            var type = this.GetType();

            return type.Name + ".delete." + objectID;
        }

        /// <summary>
        /// Gets cache key for "delete" object action
        /// </summary>
        /// <param name="objectID">ObjectID of deleted object (primary key)</param>
        /// <returns>Cache key for deleting an object of current class</returns>
        public string KeyDelete(string objectID)
        {
            var type = this.GetType();

            return type.Name + ".delete." + objectID;
        }

        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object of current class</returns>
        public string KeyUpdate(int objectID)
        {
            var type = this.GetType();

            return type.Name + ".update." + objectID;
        }


        /// <summary>
        /// Gets cache key for "update" object action
        /// </summary>
        /// <param name="objectID">ObjectID of updated object (primary key)</param>
        /// <returns>Cache key for updating an object of current class</returns>
        public string KeyUpdate(string objectID)
        {
            var type = this.GetType();

            return type.Name + ".update." + objectID;
        }

        /// <summary>
        /// Gets cache key for update any object action
        /// </summary>
        /// <returns>Cache key for updating any object of given type</returns>
        public string KeyUpdateAny()
        {
            var type = this.GetType();

            return type.Name + ".updateAny";
        }

        /// <summary>
        /// Gets cache key for delete any object action
        /// </summary>
        /// <returns>Cache key for deleting any object of given type</returns>
        public string KeyDeleteAny()
        {
            var type = this.GetType();

            return type.Name + ".deleteAny";
        }


        /// <summary>
        /// Gets cache key for creation of any object of given type
        /// </summary>
        /// <returns>Cache key for creating any object of given type</returns>
        public string KeyCreateAny()
        {
            var type = this.GetType();

            return type.Name + ".createAny";
        }

        #endregion
    }
}
