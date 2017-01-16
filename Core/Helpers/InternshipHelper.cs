using Core.Helpers.Internship;
using System;

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
                        return StringHelper.GetPluralWord(duration, "0 dnů", "den", "dny", "dnů");
                    case InternshipDurationTypeEnum.Weeks:
                        return StringHelper.GetPluralWord(duration, "0 týdnů", "týden", "týdny", "týdnů");
                    // ReSharper disable once RedundantCaseLabel
                    case InternshipDurationTypeEnum.Months:
                    default:
                        return StringHelper.GetPluralWord(duration, "0 měsíců", "měsíc", "měsíce", "měsíců");
                }
            };

            if (minDurationType == maxDurationType && minValue == maxValue)
            {
                // min and max value are exactly the same - the internship has a fixed length (eg. 7-7 days)
                return $"{minValue} {translate(maxDurationType, maxValue)}";
            }

            if (minDurationType == maxDurationType)
            {
                // types are same, but min and max value are different (eg. 4-7 days)
                return $"{minValue} - {maxValue} {translate(maxDurationType, maxValue)}";
            }

            return $"{minValue} {translate(minDurationType, minValue)} - {maxValue} {translate(maxDurationType, maxValue)}";
        }

    }
}
