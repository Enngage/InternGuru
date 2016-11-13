using Core.Context;
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
        InternshipBuilder internshipBuilder;

        public InternshipController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            this.internshipBuilder = internshipBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await internshipBuilder.BuildDetailViewAsync(id ?? 0);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
    }
}