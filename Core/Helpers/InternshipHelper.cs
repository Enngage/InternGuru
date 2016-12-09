using Core.Helpers.Internship;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
    public static class InternshipHelper
    {

        /// <summary>
        /// Gets the proper duration value based on the duration type
        /// Eg: if duration type = Month the durationInMonts is retrieved
        /// </summary>
        /// <param name="durationType"></param>
        /// <param name="durationInDays"></param>
        /// <param name="durationInWeeks"></param>
        /// <param name="durationInMonts"></param>
        /// <returns>Proper duration value</returns>
        public static int GetInternshipDurationDefaultValue(InternshipDurationTypeEnum durationType, int durationInDays, int durationInWeeks, int durationInMonts)
        {
            switch (durationType)
            {
                case InternshipDurationTypeEnum.Days:
                    return durationInDays;
                case InternshipDurationTypeEnum.Weeks:
                    return durationInWeeks;
                case InternshipDurationTypeEnum.Months:
                    return durationInMonts;
                default:
                    throw new NotSupportedException($"Cannot convert {durationType} to proper duration. Duration type is not supported.");
            }
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
                        return StringHelper.GetPluralWord(duration, "den", "den", "dny", "dnů");
                    case InternshipDurationTypeEnum.Weeks:
                        return StringHelper.GetPluralWord(duration, "týden", "týden", "týdny", "týdnů");
                    case InternshipDurationTypeEnum.Months:
                    default:
                        return StringHelper.GetPluralWord(duration, "měsíc", "měsíc", "měsíce", "měsíců");
                }
            };

            if (minDurationType == maxDurationType)
            {
                return $"{minValue} - {maxValue} {translate(minDurationType, maxValue)}";
            }

            return $"{minValue} {translate(minDurationType, minValue)} - {maxValue} {translate(maxDurationType, maxValue)}";
        }

    }
}
