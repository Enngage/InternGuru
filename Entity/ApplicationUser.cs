using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Helpers;
using Entity.Base;

namespace Entity
{

    public class ApplicationUser : IdentityUser, IEntity, IEntityWithoutCodeName
    {
        #region DB Properties

        [MaxLength(150)]
        public string FirstName { get; set; }
        [MaxLength(150)]
        public string LastName { get; set; }
        [MaxLength(30)]
        public string Nickname { get; set; }
        [Required]
        public bool IsCompany{ get; set; }
        [Required]
        public bool IsCandidate{ get; set; }

        #endregion

        #region Not mapped properties

        [NotMapped]
        public int ID => 0;

        [NotMapped]
        public string CodeName {
            get
            {
                // application user does not have a code name
                return null;
            }
            // ReSharper disable once ValueParameterNotUsed
            set
            {
                // do nothing
            }}

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
        /// Gets display name of user based on first name, last name , user name and nickname
        /// Note: E-mail address is always modified to remove domain (everything after "@")
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="nickname">nickname</param>
        /// <param name="userName">userName</param>
        /// <returns>Display name of user</returns>
        public static string GetDisplayName(string firstName, string lastName, string nickname, string userName)
        {
            if (!string.IsNullOrEmpty(nickname))
            {
                return nickname;
            }

            if (!string.IsNullOrEmpty(firstName))
            {
                return $"{firstName} {lastName}";
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            return StringHelper.RemoveDomainFromEmailAddress(userName);
        }

        #endregion

        #region Virtual properties

        [NotMapped]
        public virtual string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public virtual string DisplayName => GetDisplayName(FirstName, LastName, Nickname, UserName);

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

        public object GetObjectID()
        {
            return Id;
        }

        public string GetCodeName()
        {
            return CodeName;
        }


        #endregion

    }
}
