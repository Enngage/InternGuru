using System;
using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Email;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class EmailController : BaseController
    {
        readonly EmailBuilder _emailBuilder;

        public EmailController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, EmailBuilder emailBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _emailBuilder = emailBuilder;
        }

        #region Actions

        [Route("Email/Preview/{guid}")]
        public async Task<ActionResult> Preview(string guid)
        {
            if (guid == null)
            {
                return HttpNotFound();
            }

            var emailGuid = Guid.Empty;

            try
            {
                emailGuid = Guid.Parse(guid);
            }
            catch
            {
                // invalid guid
                return HttpNotFound();
            }

            var model = await _emailBuilder.BuildEmailPreviewAsync(emailGuid);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
       
    }
}