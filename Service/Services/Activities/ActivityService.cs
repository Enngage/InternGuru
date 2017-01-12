﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Activities.Enums;
using Service.Services.Logs;

namespace Service.Services.Activities
{
    public class ActivityService :  BaseService<Activity>, IActivityService
    {

        public ActivityService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var activity = AppContext.Activities.Find(id);

            if (activity != null)
            {
                // delete activity
                AppContext.Activities.Remove(activity);

                // touch cache keys
                TouchDeleteKeys(activity);

                // fire event
                OnDelete(activity);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Activity> GetAsync(int id)
        {
            return AppContext.Activities.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Activity> GetAll()
        {
            return AppContext.Activities;
        }

        public IQueryable<Activity> GetSingle(int id)
        {
            return AppContext.Activities.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Activity obj)
        {
            AppContext.Activities.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Activity obj)
        {
            var activity = AppContext.Activities.Find(obj.ID);

            if (activity == null)
            {
                throw new NotFoundException($"Activity with ID: {obj.ID} not found");
            }

            // fire event
            OnUpdate(obj, activity);

            // update log
            AppContext.Entry(activity).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(activity);

            // save changes
            return AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Activity>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

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
    }
}