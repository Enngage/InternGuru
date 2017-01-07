using System.Web.Mvc;
using Core.Helpers.Privilege;

namespace Web.Lib.Privilege
{

    /// <summary>
    /// AuthorizeRoles attribute used for MVC controllers/actions
    /// </summary>
    public class AuthorizeRolesMvc : AuthorizeAttribute
    {
        public AuthorizeRolesMvc(params PrivilegeLevel[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}