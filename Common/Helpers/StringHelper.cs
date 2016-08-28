

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
    }
}
