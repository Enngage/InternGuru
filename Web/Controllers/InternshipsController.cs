using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Abstract;
using UI.Builders.Internship;
using UI.Builders.Master;

namespace Web.Controllers
{
    public class InternshipsController : BaseController
    {
        InternshipBuilder internshipBuilder;

        public InternshipsController(IAppContext appContext, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, masterBuilder)
        {
            this.internshipBuilder = internshipBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? page)
        {
            var model = await internshipBuilder.BuildBrowseViewAsync(page);

            return View(model);
        }

        #endregion
    }
}