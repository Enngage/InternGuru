using Core.Helpers.Internship;
using Entity;

namespace Service.Services.Internships
{
    public interface IInternshipDurationTypeService : IService<InternshipDurationType>
    {
        /// <summary>
        /// Gets duration in weeks from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in weeks</returns>
        int GetDurationInWeeks(InternshipDurationTypeEnum durationType, int duration);

        /// <summary>
        /// Gets duration in days from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in days</returns>
        int GetDurationInDays(InternshipDurationTypeEnum durationType, int duration);

        /// <summary>
        /// Gets duration in months from given duration type
        /// </summary>
        /// <param name="durationType">Duration type (month, day, week..)</param>
        /// <param name="duration">Duration</param>
        /// <returns>Duration in months</returns>
        int GetDurationInMonths(InternshipDurationTypeEnum durationType, int duration);
    }
}