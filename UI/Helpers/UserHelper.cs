using Entity;

namespace UI.Helpers
{
    public static class UserHelper
    {
        /// <summary>
        /// Gets display name of user based on first name, last name , user name and nickname
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="nickname">nickname</param>
        /// <param name="userName">userName</param>
        /// <returns>Display name of user</returns>
        public static string GetDisplayName(string firstName, string lastName, string nickname, string userName)
        {
            return ApplicationUser.GetDisplayName(firstName, lastName, nickname, userName);
        }
    }
}
