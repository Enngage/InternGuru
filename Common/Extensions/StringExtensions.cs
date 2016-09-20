using System;

namespace Common.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string src, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(toCheck))
            {
                return false;
            }
            return src.IndexOf(toCheck, comp) >= 0;
        }
    }
}
