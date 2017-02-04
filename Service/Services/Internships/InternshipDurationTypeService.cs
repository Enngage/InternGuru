using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Core.Helpers.Internship;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Internships
{
    public class InternshipDurationTypeService :  BaseService<InternshipDurationType>, IInternshipDurationTypeService
    {

        public InternshipDurationTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var durationType = AppContext.InternshipDurationTypes.Find(id);

            if (durationType != null)
            {
                // delete durationType
                AppContext.InternshipDurationTypes.Remove(durationType);

                // touch cache keys
                TouchDeleteKeys(durationType);

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, durationType);
            }

            return Task.FromResult(0);
        }

        public Task<InternshipDurationType> GetAsync(int id)
        {
            return AppContext.InternshipDurationTypes.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<InternshipDurationType> GetAll()
        {
            return AppContext.InternshipDurationTypes;
        }

        public IQueryable<InternshipDurationType> GetSingle(int id)
        {
            return AppContext.InternshipDurationTypes.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(InternshipDurationType obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.InternshipDurationTypes.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            return SaveChangesAsync(SaveEventType.Insert, obj);
        }

        public Task<int> UpdateAsync(InternshipDurationType obj)
        {
            var durationType = AppContext.InternshipDurationTypes.Find(obj.ID);

            if (durationType == null)
            {
                throw new NotFoundException($"InternshipDurationType with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(durationType).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(durationType);

            // save changes
            return SaveChangesAsync(SaveEventType.Update, obj, durationType);
        }

        public async Task<IEnumerable<InternshipDurationType>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

        /// <summary>
        /// Gets duration in weeks from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in weeks</returns>
        public int GetDurationInWeeks(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // no need to convert
                    return duration;
                case InternshipDurationTypeEnum.Days:
                    // convert Days to Weeks
                    return (int)duration / 7;
                case InternshipDurationTypeEnum.Months:
                    // convert Months to Weeks
                    return (int)duration * 4;
                default:
                    throw new ArgumentException("Invalid duration type");
            }

        }

        /// <summary>
        /// Gets duration in days from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in days</returns>
        public int GetDurationInDays(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // convert weeks to days
                    return duration * 7;
                case InternshipDurationTypeEnum.Days:
                    // no need to convert
                    return duration;
                case InternshipDurationTypeEnum.Months:
                    // convert Months to Days
                    return duration * 30;
                default:
                    throw new ArgumentException("Invalid duration type");
            }
        }

        /// <summary>
        /// Gets duration in months from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in months</returns>
        public int GetDurationInMonths(InternshipDurationTypeEnum durationType, int duration)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Weeks:
                    // convert weeks to monts
                    return (int)duration / 4;
                case InternshipDurationTypeEnum.Days:
                    // convert Days to Months
                    return (int)duration / 30;
                case InternshipDurationTypeEnum.Months:
                    // no need to convert
                    return duration;
                default:
                    throw new ArgumentException("Invalid duration type");
            }
        }
    }
}
