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
        private readonly MasterBuilder _masterBuilder;

        #endregion

        #region Constructor

        public HomeController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder,
            HomeBuilder homeBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _homeBuilder = homeBuilder;
            _masterBuilder = masterBuilder;
        }

#endregion

        #region Actions

        public ActionResult Index()
        {
            var model = _masterBuilder.GetMasterModel();

            return View(model);
        }

        public ActionResult Pricing()
        {
            var model = _masterBuilder.GetMasterModel();

            return View(model);
        }


        public ActionResult About()
        {
            var model = _masterBuilder.GetMasterModel();

            return View(model);
        }


        public ActionResult Info()
        {
            var model = _masterBuilder.GetMasterModel();

            return View(model);
        }


        public ActionResult Contact()
        {
            var model = _homeBuilder.BuildContactUsView();

            return View(model);
        }

        #endregion

        #region POST methods

        [HttpPost]
        [ValidateAntiForgeryToken]
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