
using Core.Helpers.Privilege;
using System.Web.Http;

namespace Web.Lib.Authorize
{
    /// <summary>
    /// AuthorizeRoles attribute used for Web API
    /// </summary>
    public class AuthorizeRolesWebApi : AuthorizeAttribute
    {
        public AuthorizeRolesWebApi(params PrivilegeLevel[] roles)
        {
            this.Roles = string.Join(",", roles);
        }
    }
}