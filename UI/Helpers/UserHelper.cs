using System.Web.Mvc;
using Entity;
using UI.Base;

namespace UI.Helpers
{
    public class UserHelper : HelperBase
    {
        public UserHelper(WebViewPage webViewPage) : base(webViewPage) { }

        /// <summary>
        /// Gets display name of user based on first name, last name , user name and nickname
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="nickname">nickname</param>
        /// <param name="userName">userName</param>
        /// <returns>Display name of user</returns>
        public string GetDisplayName(string firstName, string lastName, string nickname, string userName)
        {
            return GetDisplayNameStatic(firstName, lastName, nickname, userName);
        }

        #region Static methods

        /// <summary>
        /// Gets display name of user based on first name, last name , user name and nickname
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="nickname">nickname</param>
        /// <param name="userName">userName</param>
        /// <returns>Display name of user</returns>
        public static string GetDisplayNameStatic(string firstName, string lastName, string nickname, string userName)
        {
            return ApplicationUser.GetDisplayName(firstName, lastName, nickname, userName);
        }

        #endregion
    }
}
