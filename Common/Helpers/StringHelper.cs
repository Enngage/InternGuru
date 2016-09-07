

using System;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class StringHelper
    {

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
            if (!String.IsNullOrEmpty(text))
            {
                // replace all spaces with a dash
                string formattedText = text.Replace(" ", "-");

                // remove all non-alphanumeric or dash chars
                Regex rgx = new Regex("[^a-zA-Z0-9 -]");

                formattedText = rgx.Replace(formattedText, "");

                // remove dash if there are 2 dashes next to each other
                int counter = 0;
                string codeName = "";
                foreach (Char character in formattedText)
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
    }
}
