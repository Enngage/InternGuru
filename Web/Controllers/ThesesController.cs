using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Thesis;
using UI.Events;

namespace Web.Controllers
{
    public class ThesesController : BaseController
    {
        ThesisBuilder thesisBuilder;

        public ThesesController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, serviceEvents, masterBuilder)
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