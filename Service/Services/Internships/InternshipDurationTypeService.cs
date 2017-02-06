using System;
using System.Data.Entity;
using Core.Helpers.Internship;
using Entity;

namespace Service.Services.Internships
{
    public class InternshipDurationTypeService :  BaseService<InternshipDurationType>, IInternshipDurationTypeService
    {

        public InternshipDurationTypeService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

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

        public override IDbSet<InternshipDurationType> GetEntitySet()
        {
            return AppContext.InternshipDurationTypes;
        }
    }
}
