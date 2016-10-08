

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
            if (DateTime.Now.AddDays(-1) > date)
            {
                return date.ToShortDateString();
            }
            else
            {
                return date.ToShortTimeString();
            }
        }
    }
}
