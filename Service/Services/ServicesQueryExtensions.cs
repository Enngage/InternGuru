
using System.Linq;
using Entity.Base;

namespace Service.Services
{

    public static class ServicesQueryExtensions
    {

        /// <summary>
        /// Filters query so that only results of current user are returned
        /// </summary>
        /// <param name="query"></param>
        /// <param name="applicationUserId">ApplicationUserId</param>
        /// <returns></returns>
        public static IQueryable<T> ForUser<T>(this IQueryable<T> query, string applicationUserId) where T : class, IEntityWithUserStamp
        {
            return query?.Where(m => m.CreatedByApplicationUserId == applicationUserId);
        }
    }
}
