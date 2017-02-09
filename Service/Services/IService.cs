using Service.Context;
using Service.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Base;

namespace Service.Services
{
    public interface IService<T> where T : class, IEntity
    {

        #region Entity actions 

        /// <summary>
        /// Inserts object into database
        /// </summary>
        /// <param name="obj">Object to insert</param>
        /// <returns>Result of the insert action</returns>
        Task<IInsertActionResult> InsertAsync(T obj);

        /// <summary>
        /// Deletes object from database
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        Task<int> DeleteAsync(int id);

        /// <summary>
        /// Updates given object based on its inner ID
        /// </summary>
        /// <param name="obj">Object to update</param>
        /// <returns>The number of objects written to the underlying database</returns>
        Task<int> UpdateAsync(T obj);

        /// <summary>
        /// Gets query to all objects of given type (does not execute SQL query)
        /// </summary>
        /// <returns>Query to all objects of given type</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all objects from db and stores the result in cache
        /// All subsequent calls get data from cache
        /// Cache is automatically cleared whenever an object of this type is updated/created/deleted
        /// </summary>
        /// <returns>DbSet of all objects</returns>
        Task<IEnumerable<T>> GetAllCachedAsync();

        /// <summary>
        /// Gets query for single object
        /// </summary>
        /// <param name="id">ID of the object</param>
        /// <returns>Query to object</returns>
        IQueryable<T> GetSingle(int id);

        /// <summary>
        /// Gets single object from database
        /// </summary>
        /// <param name="id">ID of the object</param>
        /// <returns>Object from database</returns>
        Task<T> GetSingleAsync(int id);

        #endregion

        #region General

        /// <summary>
        /// Used for refreshing context may bring better performance when bulk inserting etc. 
        /// USE WITH CAUTION because the old context is lost
        /// </summary>
        /// <param name="appContext"></param>
        void RefreshAppContext(IAppContext appContext);

        /// <summary>
        /// Touches given key in order to release it from memory
        /// </summary>
        /// <param name="key"></param>
        void TouchKey(string key);

        #endregion

        #region Events

        event EventHandler<InsertEventArgs<T>> OnInsertBeforeObject;
        event EventHandler<UpdateEventArgs<T>> OnUpdateBeforeObject;
        event EventHandler<DeleteEventArgs<T>> OnDeleteBeforeObject;

        event EventHandler<InsertEventArgs<T>> OnInsertAfterObject;
        event EventHandler<UpdateEventArgs<T>> OnUpdateAfterObject;
        event EventHandler<DeleteEventArgs<T>> OnDeleteAfterObject;

        #endregion

    }
}
