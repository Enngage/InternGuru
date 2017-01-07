using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class InternshipsController : BaseController
    {
        readonly InternshipBuilder _internshipBuilder;

        public InternshipsController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _internshipBuilder = internshipBuilder;
        }

        #region Actions


        public async Task<ActionResult> Index(int? page, string category, string search, string city)
        {
            var model = await _internshipBuilder.BuildBrowseViewAsync(page, category, search, city);

            return View(model);
        }

        #endregion
    }
}