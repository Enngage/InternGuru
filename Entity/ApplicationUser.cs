using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Entity.Base;

namespace Entity
{

    public class ApplicationUser : IdentityUser, IEntity
    {
        #region DB Properties

        [MaxLength(150)]
        public string FirstName { get; set; }
        [MaxLength(150)]
        public string LastName { get; set; }

        [NotMapped]
        public virtual string FullName => $"{FirstName} {LastName}";

        #endregion

        #region Methods

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        #endregion

        #region Virtual properties

        #endregion

        #region Files - Avatar

        /// <summary>
        /// Gets file name of user's avatar
        /// </summary>
        /// <returns>Filename used for user's avatar</returns>
        public static string GetAvatarFileName()
        {
            return Core.Config.FileConfig.DefaultAvatarName;
        }

        /// <summary>
        /// Gets avatar folder path
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        /// <returns>Path for user avatars</returns>
        public static string GetAvatarFolderPath(string applicationUserId)
        {
            return GetUserBaseFolderPath(applicationUserId) + "/" + Core.Config.FileConfig.AvatarFolderName;
        }

        /// <summary>
        /// Base user folder path
        /// </summary>
        /// <param name="applicationUserId">applicationUserId</param>
        /// <returns>Path to base user folder</returns>
        public static string GetUserBaseFolderPath(string applicationUserId)
        {
            return Core.Config.FileConfig.BaseUserFolderPath + "/" + applicationUserId;
        }

        #endregion

        #region IEntity

        public string GetCodeName()
        {
            return UserName;
        }

        public object GetObjectID()
        {
            return Id;
        }

        #endregion

    }
}
