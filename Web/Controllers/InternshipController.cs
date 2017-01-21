using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class InternshipController : BaseController
    {
        readonly InternshipBuilder _internshipBuilder;

        public InternshipController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _internshipBuilder = internshipBuilder;
        }

        #region Actions

        [Route("Staz/{id}/{codeName}")]
        public async Task<ActionResult> Index(int? id, string codeName)
        {
            const string notFoundView = "~/Views/Internship/NotFound.cshtml";

            if (id == null)
            {
                // internship was not found
                var notFoundModel = await _internshipBuilder.BuildNotFoundViewAsync();
                return View(notFoundView, notFoundModel);
            }

            var model = await _internshipBuilder.BuildDetailViewAsync((int) id);

            if (model == null)
            {
                // internship was not found
                var notFoundModel = await _internshipBuilder.BuildNotFoundViewAsync();
                return View(notFoundView, notFoundModel);
            }

            // activity - internship view
            await _internshipBuilder.LogInternshipViewActivityAsync(model.Internship.ID, model.Internship.Company.CompanyID);

            return View(model);
        }

        #endregion
    }
}