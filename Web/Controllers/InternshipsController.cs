using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Helpers;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Internship.Enums;
using UI.Builders.Master;
using UI.Events;
using UI.Helpers;

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

      
        [Route("Staze/{category?}", Order = 1)]
        [Route("Staze", Order = 2)]
   
        public async Task<ActionResult> Index(int? page, string category, string search, string city, string paid, string length, string order)
        {
            var model = await _internshipBuilder.BuildBrowseViewAsync(page, category, search, city, EnumHelper.ParseEnum<InternshipPaidFilterEnum>(paid, InternshipPaidFilterEnum.Any), EnumHelper.ParseEnum<InternshipLengthFilterEnum>(length, InternshipLengthFilterEnum.Any), EnumHelper.ParseEnum<InternshipOrderFilterEnum>(order, InternshipOrderFilterEnum.Newest));

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [Route("Staze/Hledat", Order = 1)]
        public async Task<ActionResult> Search(int? page, string category, string search, string city, string paid, string length, string order)
        {
            var model = await _internshipBuilder.BuildBrowseViewAsync(page, category, search, city, EnumHelper.ParseEnum<InternshipPaidFilterEnum>(paid, InternshipPaidFilterEnum.Any), EnumHelper.ParseEnum<InternshipLengthFilterEnum>(length, InternshipLengthFilterEnum.Any), EnumHelper.ParseEnum<InternshipOrderFilterEnum>(order, InternshipOrderFilterEnum.Newest));

            if (model == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Internships/Index.cshtml", model);
        }

        #endregion
    }
}