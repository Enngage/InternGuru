using Service.Context;
using System.Web.Mvc;
using UI.Builders.Master;
using UI.Builders.Master.Views;
using UI.Events;
using UI.ModelState;

namespace UI.Base
{
    public abstract class BaseController : Controller
    {
        #region Variables

        private IModelState _modelStateWrapper;
        private readonly MasterBuilder _masterBuilder;

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        public IAppContext AppContext { get; }

        /// <summary>
        /// Service events 
        /// </summary>
        public IServiceEvents ServiceEvents { get; }

        /// <summary>
        /// Model state 
        /// </summary>
        public IModelState ModelStateWrapper
        {
            get
            {
                if (_modelStateWrapper == null)
                {
                    var modelStateWrapper = new ModelStateWrapper(ModelState);
                    _modelStateWrapper = modelStateWrapper;
                    return modelStateWrapper;
                }
                return _modelStateWrapper;
            }
        }

        #endregion

        #region Constructors

        protected BaseController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder)
        {
            AppContext = appContext;
            _masterBuilder = masterBuilder;
            ServiceEvents = serviceEvents;
        }

        #endregion

        #region Filters

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Register service events
            ServiceEvents.RegisterEvents(this._masterBuilder.CurrentUser, filterContext?.HttpContext?.Request?.Url?.ToString());
        }

        /// <summary>
        /// This filter is used to populate global data for MasterView
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            var model = filterContext.Controller.ViewData.Model as MasterView;

            if (model != null)
            {
                // set master property of an existing model
                model.Master = _masterBuilder.GetMasterModel();

            }
            else
            {
                // create new master model
                var masterView = new MasterView()
                {
                    Master = _masterBuilder.GetMasterModel()
                };

                // set model
                filterContext.Controller.ViewData.Model = masterView;
            }
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AppContext.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
