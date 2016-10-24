using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Thesis;

namespace Web.Controllers
{
    public class ThesesController : BaseController
    {
        ThesisBuilder thesisBuilder;

        public ThesesController(IAppContext appContext, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, masterBuilder)
        {
            this.thesisBuilder = thesisBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? page, string category, string search)
        {
            var model = await thesisBuilder.BuildBrowseViewAsync(search, page, category);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
    }
}