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


        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _internshipBuilder.BuildDetailViewAsync((int) id);

            if (model == null)
            {
                return HttpNotFound();
            }

            // activity - internship view
            await _internshipBuilder.LogInternshipViewActivityAsync(model.Internship.ID, model.Internship.Company.CompanyID);

            return View(model);
        }

        #endregion
    }
}