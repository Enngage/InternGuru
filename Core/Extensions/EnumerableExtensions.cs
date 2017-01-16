using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Gets random value from IEnumerable collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">Data</param>
        /// <returns>Random value from IEnumerable collection</returns>
        public static T RandomItem<T>(this IEnumerable<T> data)
        {
            return data.RandomItem(new Random());
        }

        /// <summary>
        /// Gets random value from IEnumerable collection
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="data">Data</param>
        /// <param name="rand">Random</param>
        /// <returns>Random value from IEnumerable collection</returns>
        public static T RandomItem<T>(this IEnumerable<T> data, Random rand)
        {
            var enumerable = data as IList<T> ?? data.ToList();
            var index = rand.Next(0, enumerable.Count());
            
            return index == 0 ? enumerable.FirstOrDefault() : enumerable.ElementAt(index);
        }
    }
}
