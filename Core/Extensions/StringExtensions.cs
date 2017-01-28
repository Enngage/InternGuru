using System;
using System.Linq;

namespace Core.Extensions
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

        public static string ToAlphaNumeric(this string src)
        {
            return new string(Array.FindAll(src.ToCharArray(), char.IsLetterOrDigit));
        }

        public static string ToAlphaNumeric(this string src, params char[] allowedCharacters)
        {
            return new string(Array.FindAll(src.ToCharArray(), c => char.IsLetterOrDigit(c) || allowedCharacters.Contains(c)));
        }
    }
}
