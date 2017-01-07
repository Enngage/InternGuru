

using System;
using System.Text.RegularExpressions;

namespace Core.Helpers
{
    public static class StringHelper
    {

        /// <summary>
        /// Indicates whether given string is well formed URL
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>True if url is well formed, false otherwise</returns>
        public static bool IsValidUrl(string url)
        {
            Uri uriResult;
            var result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        /// <summary>
        /// Shortens given text
        /// </summary>
        /// <param name="appendDots">If true, 3 dots "..." will be appended to text</param>
        /// <param name="text"></param>
        /// <param name="count">Number of characters to return</param>
        /// <returns>Shortened text</returns>
        public static string ShortenText(string text, int count, bool appendDots = true)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (text.Length < count)
            {
                return text;
            }

            var shortenedText = text.Substring(0, count);

            if (appendDots)
            {
                return shortenedText + "...";
            }
            else
            {
                return shortenedText;
            }
        }

        /// <summary>
        /// Gets plural version of given word based on count
        /// Use {count} in version properties to replace it with the actual count
        /// </summary>
        /// <param name="count">Count</param>
        /// <param name="version0">Version of the word when the count = 0</param>
        /// <param name="version1">Version of the word when the count = 1</param>
        /// <param name="version2">Version of the word when the count = 2</param>
        /// <param name="version5">Version of the word when the count = 5</param>
        /// <returns>True if string is valid e-mail address, false otherwise</returns>
        public static string GetPluralWord(int count, string version0, string version1, string version2, string version5)
        {
            var modCount = count % 10;

            Func<string, string> replaceCount = (version) =>
               {
                   return version.Replace("{count}", count.ToString());
               };
            
            if (modCount == 0)
            {
                return replaceCount(version0);
            }
            else if (modCount == 1)
            {
                return replaceCount(version1);
            }
            else if (modCount > 1 && modCount < 5)
            {
                return replaceCount(version2);
            }
            else
            {
                return replaceCount(version5);
            }
        }

        /// <summary>
        /// Indicates if given string is a valid e-mail address
        /// </summary>
        /// <param name="email">Input string</param>
        /// <returns>True if string is valid e-mail address, false otherwise</returns>
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets code name from given string by replacing all non alpha-numeric characters with dash (including white spaces)
        /// </summary>
        /// <param name="text">Text to transfer</param>
        /// <returns>Code name from given text</returns>
        public static string GetCodeName(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                // replace all spaces with a dash
                var formattedText = text.Replace(" ", "-");

                // remove all non-alphanumeric or dash chars
                var rgx = new Regex("[^a-zA-Z0-9 -]");

                formattedText = rgx.Replace(formattedText, "");

                // remove dash if there are 2 dashes next to each other
                var counter = 0;
                var codeName = "";
                foreach (var character in formattedText)
                {
                    if (character == '-')
                    {
                        counter++;

                        if (counter == 1)
                        {
                            codeName += character;
                        }
                        if (counter >= 2)
                        {
                            // don't add the character
                        }
                    }
                    else
                    {
                        // add character to string
                        codeName += character;

                        // reset counter
                        counter = 0;
                    }
                }

                return codeName;
            }
            return null;
        }

        /// <summary>
        /// Formats number. 
        /// Returns "9,593,938" instead of "9593938"
        /// </summary>
        /// <param name="number">Number to format</param>
        /// <returns>Formatted number as string</returns>
        public static string FormatNumber(long number)
        {
            return number.ToString("#,##0");
        }

        /// <summary>
        /// Formats number. 
        /// Returns "9,593,938" instead of "9593938"
        /// </summary>
        /// <param name="number">Number to format</param>
        /// <returns>Formatted number as string</returns>
        public static string FormatNumber(double number)
        {
            return FormatNumber((long)number);
        }

        /// <summary>
        /// Formats number. 
        /// Returns "9,593,938" instead of "9593938"
        /// </summary>
        /// <param name="number">Number to format</param>
        /// <returns>Formatted number as string</returns>
        public static string FormatNumber(int number)
        {
            return FormatNumber((double)number);
        }
    }
}
