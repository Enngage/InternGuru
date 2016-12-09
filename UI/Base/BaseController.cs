using Service.Context;
using System.Web.Mvc;
using UI.Builders.Master;
using UI.Builders.Master.Views;
using UI.Events;
using UI.ModelState;

namespace UI.Base
{
    public abstract class BaseController: Controller
    {
        #region Variables

        private IAppContext appContext;
        private IModelState modelStateWrapper;
        private MasterBuilder masterBuilder;
        private IServiceEvents serviceEvents;

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        public IAppContext AppContext
        {
            get
            {
                return this.appContext;
            }
        }

        /// <summary>
        /// Service events 
        /// </summary>
        public IServiceEvents ServiceEvents
        {
            get
            {
                return this.serviceEvents;
            }
        }

        /// <summary>
        /// Model state 
        /// </summary>
        public IModelState ModelStateWrapper
        {
            get
            {
                if (this.modelStateWrapper == null)
                {
                    var modelStateWrapper = new ModelStateWrapper(this.ModelState);
                    this.modelStateWrapper = modelStateWrapper;
                    return modelStateWrapper;
                }
                return this.modelStateWrapper;
            }
        }

        #endregion

        #region Constructors

        public BaseController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder)
        {
            this.appContext = appContext;
            this.masterBuilder = masterBuilder;
            this.serviceEvents = serviceEvents;
        }

        #endregion

        #region Filters

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Register service events
            this.ServiceEvents.RegisterEvents();
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
                model.Master = this.masterBuilder.GetMasterModel();
            }
            else
            {
                // create new master model
                var masterView = new MasterView()
                {
                    Master = this.masterBuilder.GetMasterModel()
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
                appContext.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
