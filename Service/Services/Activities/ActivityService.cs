using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Services.Activities.Enums;

namespace Service.Services.Activities
{
    public class ActivityService :  BaseService<Activity>, IActivityService
    {
        public ActivityService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public IQueryable<Activity> GetActivities(ActivityTypeEnum type)
        {
            return this.GetAll().Where(m => m.ActivityType.Equals(type.ToString()));
        }

        public IQueryable<Activity> GetActivities(ActivityTypeEnum type, int companyID)
        {
            return this.GetAll().Where(m => m.ActivityType.Equals(type.ToString()) && m.RelevantCompanyID.Equals(companyID));
        }

        public IQueryable<Activity> GetActivities(ActivityTypeEnum type, string applicationUserId)
        {
            return this.GetAll().Where(m => m.ActivityType.Equals(type.ToString()) && m.ApplicationUserId.Equals(applicationUserId));
        }

        public async Task<int> LogActivity(ActivityTypeEnum type)
        {
            return await LogActivity(type, 0, null);
        }

        public async Task<int> LogActivity(ActivityTypeEnum type, int companyID, int objectID = 0)
        {
            return await LogActivity(type, companyID, null, objectID);
        }

        public async Task<int> LogActivity(ActivityTypeEnum type, string applicationUserId, int objectID = 0)
        {
            return await LogActivity(type, 0, applicationUserId, objectID);
        }

        public async Task<int> LogActivity(ActivityTypeEnum type, int companyID, string applicationUserId, int objectID = 0)
        {
            var activity = new Activity()
            {
                ActivityType = type.ToString(),
                ActivityDateTime = DateTime.Now,
                ApplicationUserId = applicationUserId,
                ObjectID = objectID,
                RelevantCompanyID = companyID,
            };

            return await InsertAsync(activity);
        }

        public IQueryable<Activity> GetInternshipFormSubmissions(int internshipID)
        {
            return GetActivities(ActivityTypeEnum.FormSubmitInternship)
                .Where(m => m.ObjectID == internshipID);
        }

        public override IDbSet<Activity> GetEntitySet()
        {
            return this.AppContext.Activities;
        }
    }
}
