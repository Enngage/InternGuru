using System.Linq;
using Entity.Base;

namespace Service.Extensions
{

    public static class QueryableExtensions
    {

        /// <summary>
        /// Filters query so that only entities of current user are returned
        /// </summary>
        /// <param name="query"></param>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns></returns>
        public static IQueryable<T> ForUser<T>(this IQueryable<T> query, string applicationUserId) where T : class, IEntityWithUserStamp
        {
            return query?.Where(m => m.CreatedByApplicationUserId == applicationUserId);
        }


        /// <summary>
        /// Filters query so that only active entities are returned
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IQueryable<T> OnlyActive<T>(this IQueryable<T> query) where T : class, IEntityWithActiveState
        {
            return query?.Where(m => m.IsActive);
        }
    }
}
