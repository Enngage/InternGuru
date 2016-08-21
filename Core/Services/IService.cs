﻿
using Core.Context;
using Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IService<T> where T : EntityAbstract
    {
        /// <summary>
        /// Inserts object into database
        /// </summary>
        /// <param name="obj">Object to insert</param>
        /// <returns></returns>
        Task InsertAsync(T obj);

        /// <summary>
        /// Deletes object from database
        /// </summary>
        /// <returns></returns>
        Task DeleteAsync(int id);

        /// <summary>
        /// Updates given object based on its inner ID
        /// </summary>
        /// <param name="obj">Object to update</param>
        /// <returns></returns>
        Task UpdateAsync(T obj);

        /// <summary>
        /// Gets query to all objects of given type (does not execute SQL query)
        /// </summary>
        /// <returns>Query to all objects of given type</returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets single object from database
        /// </summary>
        /// <param name="id">ID of the object</param>
        /// <returns>Object from database</returns>
        Task<T> Get(int id);

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

    }
}
