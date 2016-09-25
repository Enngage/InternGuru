

using System;

namespace Common.Helpers
{
    public static class DateHelper
    {
        /// <summary>
        /// Gets pretty formated date
        /// </summary>
        /// <param name="date">Date to format</param>
        /// <returns>Formatted date</returns>
        public static string GetPrettyDate(DateTime date)
        {
            if (date.AddDays(-1) < DateTime.Now)
            {
                return date.ToShortTimeString();
            }
            else
            {
                return date.ToShortDateString();
            }
        }
    }
}
