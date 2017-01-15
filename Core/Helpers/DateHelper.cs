using System;

namespace Core.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Gets formated date value
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <returns>Formatted date</returns>
        public static string FormatDate(DateTime date)
        {
            return date.ToString("d/M/yyyy");
        }

        /// <summary>
        /// Gets formated time value
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <returns>Formatted time</returns>
        public static string FormatTime(DateTime date)
        {
            return DateTime.Now.AddDays(-1) > date ? FormatDate(date) : date.ToString("H:m");
        }
    }
}