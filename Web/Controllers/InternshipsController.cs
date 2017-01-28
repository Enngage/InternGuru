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

      
        [Route("Staze/{category:alpha?}/{page:int?}", Order = 1)]
        [Route("Staze/{page:int}", Order = 2)]
        [Route("Staze", Order = 3)]
   
        public async Task<ActionResult> Index(int? page, string category, string search, string city)
        {
            var model = await _internshipBuilder.BuildBrowseViewAsync(page, category, search, city);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [Route("Staze/Hledat", Order = 1)]
        public async Task<ActionResult> Search(int? page, string category, string search, string city)
        {
            var model = await _internshipBuilder.BuildBrowseViewAsync(page, category, search, city);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Internships/Index.cshtml", model);
        }

        #endregion
    }
}