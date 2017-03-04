using System;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Events;
using UI.Helpers;

namespace Web.Api
{
    public class UserController : BaseApiController
    {

        public UserController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
        }

        #region Actions

        [System.Web.Http.HttpPost]
        public IHttpActionResult GetAvatarOfCurrentUser()
        {
            try
            {
                if (!MasterBuilder.CurrentUser.IsAuthenticated)
                {
                    return BadRequest("You need to be logged for this action");
                }

                var avatar = ImageHelper.GetUserAvatarStatic(MasterBuilder.CurrentUser.Id);

                return Ok(avatar);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}