using Common.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Helpers
{
    public static class InternshipHelper
    {

        /// <summary>
        /// Gets collection of allowed amount types (e.g. "Total", "Per month" ..)
        /// </summary>
        /// <returns>Collection of duration types</returns>
        public static IEnumerable<string> GetAmountTypes()
        {
            return new List<string>()
            {
                "Celkem",
                "Měsíc"
            };
        }

        /// <summary>
        /// Gets collection of allowed duration types (e.g. "Days", "Month" ..)
        /// </summary>
        /// <returns>Collection of duration types</returns>
        public static IEnumerable<InternshipDurationTypeModel> GetInternshipDurations()
        {
            return new List<InternshipDurationTypeModel>()
            {
                new InternshipDurationTypeModel()
                {
                    DurationName = "Měsíců",
                    DurationValue = InternshipDurationTypeEnum.Months
                },
                new InternshipDurationTypeModel()
                {
                    DurationName = "Týdnů",
                    DurationValue = InternshipDurationTypeEnum.Weeks,
                },
                new InternshipDurationTypeModel()
                {
                    DurationName = "Dnů",
                    DurationValue = InternshipDurationTypeEnum.Days,
                }
            };
        }

        /// <summary>
        /// Get internship duration type
        /// </summary>
        /// <param name="durationValue">Duration value (codeName)</param>
        public static InternshipDurationTypeModel GetInternshipDuration(InternshipDurationTypeEnum durationValue)
        {
            return GetInternshipDurations().Where(m => m.DurationValue.Equals(durationValue)).FirstOrDefault();
        }

        /// <summary>
        /// Gets "nice" text (eg" 3 - 5 týdnů, 1 -2 týdny, 3 týdny - 5 měsíců)
        /// </summary>
        /// <param name="minDurationType"></param>
        /// <param name="minValue"></param>
        /// <param name="maxDurationType"></param>
        /// <param name="maxValue"></param>
        public static string GetInternshipDurationDisplayValue(InternshipDurationTypeEnum minDurationType, int minValue, InternshipDurationTypeEnum maxDurationType, int maxValue)
        {
            Func<InternshipDurationTypeEnum, int, string> translate = (type, duration) =>
            {
                switch (type)
                {
                    case InternshipDurationTypeEnum.Days:
                        return StringHelper.GetPluralWord(duration, "den", "dny", "dnů");
                    case InternshipDurationTypeEnum.Weeks:
                        return StringHelper.GetPluralWord(duration, "týden", "týdny", "týdnů");
                    case InternshipDurationTypeEnum.Months:
                    default:
                        return StringHelper.GetPluralWord(duration, "měsíc", "měsíce", "měsíců");
                }
            };

            if (minDurationType == maxDurationType)
            {
                return $"{minValue} - {maxValue} {translate(minDurationType, maxValue)}";
            }

            return $"{minValue} {translate(minDurationType, minValue)} - {maxValue} {translate(maxDurationType, maxValue)}";
        }

        /// <summary>
        /// Gets collection of possible employee count states
        /// </summary>
        /// <returns>Collection of count states</returns>
        public static IEnumerable<InternshipCompanySizeModel> GetAllowedCompanySizes()
        {
            return new List<InternshipCompanySizeModel>()
            {
              new InternshipCompanySizeModel()
              {
                  Name = "< 10",
                  Value = "9",
              },
               new InternshipCompanySizeModel()
              {
                  Name = "10 - 19",
                  Value = "19"
              },
                new InternshipCompanySizeModel()
              {
                  Name = "20 - 49",
                  Value = "49"
              },
                 new InternshipCompanySizeModel()
              {
                  Name = "50 - 99",
                  Value = "99"
              },
                  new InternshipCompanySizeModel()
              {
                  Name = "100+",
                  Value = "100"
              },
           };
        }
    }
}
