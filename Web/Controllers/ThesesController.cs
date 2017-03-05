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
        readonly ThesisBuilder _thesisBuilder;

        public ThesesController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _thesisBuilder = thesisBuilder;
        }

        #region Actions


        [Route("ZaverecnePrace/{category?}", Order = 1)]
        [Route("ZaverecnePrace", Order = 2)]
        public async Task<ActionResult> Index(int? page, string category, string search)
        {
            var model = await _thesisBuilder.BuildBrowseViewAsync(search, page, category);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }


        [Route("ZaverecnePrace/Hledat", Order = 1)]
        public async Task<ActionResult> Search(int? page, string category, string search)
        {
            var model = await _thesisBuilder.BuildBrowseViewAsync(search, page, category);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Theses/Index.cshtml", model);
        }

        #endregion
    }
}