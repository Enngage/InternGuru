using Core.Helpers.Privilege;
using System.Web.Mvc;

namespace Web.Lib.Authorize
{

    /// <summary>
    /// AuthorizeRoles attribute used for MVC controllers/actions
    /// </summary>
    public class AuthorizeRolesMVC : AuthorizeAttribute
    {
        public AuthorizeRolesMVC(params PrivilegeLevel[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}