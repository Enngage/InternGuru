using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Thesis;
using UI.Events;

namespace Web.Controllers
{
    public class ThesisController : BaseController
    {
        readonly ThesisBuilder _thesisBuilder;

        public ThesisController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _thesisBuilder = thesisBuilder;
        }

        #region Actions

        [Route("ZaverecnaPrace/{id}/{codeName}")]
        public async Task<ActionResult> Index(int? id)
        {
            const string notFoundView = "~/Views/Thesis/NotFound.cshtml";

            if (id == null)
            {
                var notFoundModel = await _thesisBuilder.BuildNotFoundViewAsync();
                return View(notFoundView, notFoundModel);
            }

            var model = await _thesisBuilder.BuildDetailViewAsync((int) id);

            if (model == null)
            {
                var notFoundModel = await _thesisBuilder.BuildNotFoundViewAsync();
                return View(notFoundView, notFoundModel);
            }

            return View(model);
        }

        #endregion
    }
}