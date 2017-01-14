using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Services.Activities.Enums;

namespace Service.Services.Activities
{
    public interface IActivityService : IService<Activity>
    {
        IQueryable<Activity> GetActivities(ActivityTypeEnum type);
        IQueryable<Activity> GetActivities(ActivityTypeEnum type, int companyID);
        IQueryable<Activity> GetActivities(ActivityTypeEnum type, string applicationUserId);

        /// <summary>
        /// Gets all activities of form submission type
        /// </summary>
        /// <param name="internshipID"></param>
        /// <returns></returns>
        IQueryable<Activity> GetInternshipFormSubmissions(int internshipID);

        Task<int> LogActivity(ActivityTypeEnum type);
        Task<int> LogActivity(ActivityTypeEnum type, int companyID, int objectID = 0);
        Task<int> LogActivity(ActivityTypeEnum type, string applicationUserId, int objectID = 0);
        Task<int> LogActivity(ActivityTypeEnum type, int companyID, string applicationUserId, int objectID = 0);

    }
}