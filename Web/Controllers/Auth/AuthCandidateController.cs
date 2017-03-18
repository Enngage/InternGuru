using System.Threading.Tasks;
using System.Web.Mvc;
using Service.Context;
using UI.Builders.Auth;
using UI.Builders.Auth.Forms;
using UI.Builders.Master;
using UI.Builders.Shared.Forms;
using UI.Enums;
using UI.Events;
using UI.Exceptions;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route(CandidateActionPrefix + "/Subscription")]
        public async Task<ActionResult> Subscription(AuthCitiesSubscriptionForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return RedirectToAction("CandidateTypeIndex", new { result = ActionResultEnum.Failure});
            }

            try
            {
                // add subscribed cities
                await AuthBuilder.AuthSubscriptionBuilder.AddSubscribedCitiesToCurrentUserAsync(form?.Cities?.Split(','));

                return RedirectToAction("CandidateTypeIndex", new { result = ActionResultEnum.Success });
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return RedirectToAction("CandidateTypeIndex", new { result = ActionResultEnum.Failure });
            }
        }

    }
}