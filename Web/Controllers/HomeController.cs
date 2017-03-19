using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using Service.Context;
using UI.Builders.Home;
using UI.Builders.Home.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        #region Builders

        private readonly HomeBuilder _homeBuilder;

        #endregion

        #region Constructor

        public HomeController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder,
            HomeBuilder homeBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _homeBuilder = homeBuilder;
        }

        #endregion

        #region Actions

        [Route("")]
        public async Task<ActionResult> Index()
        {
            var model = await _homeBuilder.BuildIndexViewAsync();

            return View(model);
        }

        [Route("Cenik")]
        public ActionResult Pricing()
        {
            return View();
        }

        [Route("Onas")]
        public ActionResult About()
        {
            return View();
        }

        [Route("Info")]
        public ActionResult Info()
        {

            return View();
        }

        [Route("Kontakt")]
        public ActionResult Contact()
        {
            var model = _homeBuilder.BuildContactUsView();

            return View(model);
        }

        [Route("Dotaznik")]
        public ActionResult Questionnaire()
        {
            return View();
        }

        #endregion

        #region POST methods

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Kontakt")]
        public async Task<ActionResult> Contact(HomeContactUsForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                return View(_homeBuilder.BuildContactUsView(form));
            }

            try
            {
                await _homeBuilder.SubmitContactUsForm(form);

                // get contact model
                var model = _homeBuilder.BuildContactUsView();

                // set form status
                model.ContactUsForm.FormResult.IsSuccess = true;

                // clear the form
                model.ContactUsForm.Message = string.Empty;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                return View(_homeBuilder.BuildContactUsView(form));
            }
        }


        #endregion
    }
}