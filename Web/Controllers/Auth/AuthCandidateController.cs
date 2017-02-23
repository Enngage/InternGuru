using System.Web.Mvc;
using Service.Context;
using UI.Builders.Auth;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers.Auth
{
    [Authorize]
    public class AuthCandidateController : AuthController
    {

        #region Constructor

        public AuthCandidateController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, AuthBuilder authBuilder) : base(appContext, serviceEvents, masterBuilder, authBuilder)
        {
        }

        #endregion

        [Route(CandidateActionPrefix + "/Todo")]
        public ActionResult Index()
        {
            return HttpNotFound();
        }

    }
}