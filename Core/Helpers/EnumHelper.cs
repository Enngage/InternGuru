using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Helpers
{
    public static class EnumHelper
    {

        /// <summary>
        /// Gets collection of all possible enum values
        /// </summary>
        /// <typeparam name="T">Type of enum</typeparam>
        /// <returns>Collection of enum states</returns>
        public static IEnumerable<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        /// <summary>
        /// Converts given string value to Enum
        /// </summary>
        /// <typeparam name="T">Enum to parse to</typeparam>
        /// <param name="value">Value to parse</param>
        /// <returns>Enum value</returns>
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        /// Converts given string value to Enum or gives default value if parsing failes
        /// </summary>
        /// <typeparam name="T">Enum to parse to</typeparam>
        /// <param name="value">Value to parse</param>
        /// <param name="defaultValue">Default value - should be enum value</param>
        /// <returns>Enum value</returns>
        public static T ParseEnum<T>(string value, T defaultValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
