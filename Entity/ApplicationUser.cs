using Common.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

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

        #region Virtual properties

        #endregion

        #region IEntity

        public string GetCodeName()
        {
            return this.UserName;
        }

        public object GetObjectID()
        {
            return this.Id;
        }

        #endregion

    }
}
