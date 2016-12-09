using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Master;
using UI.Builders.Thesis;
using UI.Events;

namespace Web.Controllers
{
    public class ThesisController : BaseController
    {
        ThesisBuilder thesisBuilder;

        public ThesisController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            this.thesisBuilder = thesisBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await thesisBuilder.BuildDetailViewAsync(id ?? 0);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion
    }
}