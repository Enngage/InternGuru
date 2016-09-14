using Common.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Entity
{

    public class ApplicationUser : IdentityUser
    {
        #region DB Properties

        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public virtual string FullName
        {
            get
            {
                return string.Format("{0} {1}", FirstName, LastName);
            }
        }

        #endregion

        #region Methods

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        /// <summary>
        /// Gets file name of user's avatar
        /// </summary>
        /// <param name="userName">User name</param>
        /// <returns>Filename used for user's avatar</returns>
        public static string GetAvatarFileName(string userName)
        {
            return StringHelper.GetCodeName(userName);
        }

        #endregion

        #region Cache keys


        /// <summary>
        /// Gets cache key for update user action
        /// </summary>
        /// <param name="userName">Name of user who was updated</param>
        /// <returns>Cache key for update user action</returns>
        public static string KeyUpdate<ApplicationUser>(string userName) 
        {
            return ConstructKey(typeof(ApplicationUser), ActionType.Update, userName);
        }

        /// <summary>
        /// Gets cache key for update of any user
        /// </summary>
        /// <returns>Cache key for update any user action</returns>
        public static string KeyUpdateAny<ApplicationUser>()
        {
            return ConstructKey(typeof(ApplicationUser), ActionType.UpdateAny);
        }

        /// <summary>
        /// Creates key from given action type and object ID 
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="actionType">Action type</param>
        /// <param name="objectID">ObjectID if necessary</param>
        /// <returns></returns>
        private static string ConstructKey(Type type, ActionType actionType, string objectID)
        {
            return string.Format("{0}.{1}[{2}]", type.FullName, actionType.Value, objectID);
        }

        /// <summary>
        /// Creates key from given action type
        /// </summary>
        /// <param name="type">Type of object</param>
        /// <param name="actionType">Action type</param>
        /// <returns></returns>
        private static string ConstructKey(Type type, ActionType actionType)
        {
            return string.Format("{0}.{1}", type.FullName, actionType.Value);
        }

        #endregion
    }
}
