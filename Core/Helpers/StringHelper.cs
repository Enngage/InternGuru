﻿

using System;
using System.Text;
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

            Func<string, string> replaceCount = (version) => version.Replace("{count}", count.ToString());

            switch (modCount)
            {
                case 0:
                    return count == 0 ? replaceCount(version0) : replaceCount(version5); 
                case 1:
                    return count == 1 ? replaceCount(version1) : replaceCount(version5);
                default:
                    if (modCount > 1 && modCount < 5 && count < 5)
                    {
                        return replaceCount(version2);
                    }
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
        /// <param name="charCount">Characters to take, if set to negative number (e.g. -1) all characters will be returned</param>
        /// <returns>Code name from given text</returns>
        public static string GetCodeName(string text, int charCount)
        {
            if (string.IsNullOrEmpty(text)) return null;

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

            return charCount < 0 ? codeName : ShortenText(codeName, charCount, false);
        }

        /// <summary>
        /// Gets code name from given string by replacing all non alpha-numeric characters with dash (including white spaces)
        /// </summary>
        /// <param name="text">Text to transfer</param>
        /// <returns>Code name from given text</returns>
        public static string GetCodeName(string text)
        {
            return GetCodeName(text, -1);
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

        /// <summary>
        /// Removes everything after "@" in a e-mail address
        /// test@email.com becomes "test"
        /// </summary>
        /// <param name="email">E-mail address</param>
        /// <returns>E-mail address without domain</returns>
        public static string RemoveDomainFromEmailAddress(string email)
        {
            var index = email.IndexOf("@", StringComparison.Ordinal);
            if (index > 0)
                email = email.Substring(0, index);

            return email;
        }

        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }


        /// <summary>
        /// Generates Lorem Ipsum text
        /// </summary>
        /// <param name="minWords">Minimum words</param>
        /// <param name="maxWords">Maximum words</param>
        /// <param name="minSentences">Minimium sentences</param>
        /// <param name="maxSentences">Maximum sentences></param>
        /// <param name="numLines">Number of lines</param>
        /// <returns></returns>
        private static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numLines)
        {
            var words = new[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetuer", "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod", "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat" };

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            var sb = new StringBuilder();
            for (int p = 0; p < numLines; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { sb.Append(" "); }
                        sb.Append(words[rand.Next(words.Length)]);
                    }
                    sb.Append(". ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }


    }
}
