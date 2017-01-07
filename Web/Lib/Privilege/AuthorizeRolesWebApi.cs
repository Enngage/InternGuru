using System.Web.Http;
using Core.Helpers.Privilege;

namespace Web.Lib.Privilege
{
    /// <summary>
    /// AuthorizeRoles attribute used for Web API
    /// </summary>
    public class AuthorizeRolesWebApi : AuthorizeAttribute
    {
        public AuthorizeRolesWebApi(params PrivilegeLevel[] roles)
        {
            Roles = string.Join(",", roles);
        }
    }
}