using Core.Context;
using System.Web.Mvc;
using UI.Builders.Master;
using UI.Builders.Master.Views;
using UI.ModelState;

namespace UI.Base
{
    public abstract class BaseController: Controller
    {
        #region Variables

        private IAppContext appContext;
        private IModelState modelStateWrapper;
        private MasterBuilder masterBuilder;

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

        public BaseController(IAppContext appContext, MasterBuilder masterBuilder)
        {
            this.appContext = appContext;
            this.masterBuilder = masterBuilder;
        }

        #endregion

        #region OnActionExecuted filter

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
                model.Master = this.masterBuilder.GetMasterModel();
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
