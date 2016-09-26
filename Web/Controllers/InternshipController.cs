using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Master;

namespace Web.Controllers
{
    public class InternshipController : BaseController
    {
        InternshipBuilder internshipBuilder;

        public InternshipController(IAppContext appContext, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, masterBuilder)
        {
            this.internshipBuilder = internshipBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int id)
        {
            var model = await internshipBuilder.BuildDetailViewAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
    }
}