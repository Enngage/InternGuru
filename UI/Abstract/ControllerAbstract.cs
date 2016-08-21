//using Core.Context;
//using System.Web.Mvc;

//namespace UI.Abstract
//{
//    public abstract class BaseControllerAbstract : Controller
//    {
//        #region Variables

//        private IAppContext appContext;
//        private ModelStateWrapper modelStateWrapper;
//        private MasterBuilder masterBuilder;

//        #endregion

//        #region Properties

//        /// <summary>
//        /// AppContext 
//        /// </summary>
//        public AppContext AppContext
//        {
//            get
//            {
//                return this.appContext;
//            }
//        }

//        /// <summary>
//        /// Model state 
//        /// </summary>
//        public ModelStateWrapper ModelStateWrapper
//        {
//            get
//            {
//                if (this.modelStateWrapper == null)
//                {
//                    var modelStateWrapper = new ModelStateWrapper(this.ModelState);
//                    this.modelStateWrapper = modelStateWrapper;
//                    return modelStateWrapper;
//                }
//                return this.modelStateWrapper;
//            }
//        }

//        #endregion

//        #region Constructors

//        public BaseControllerAbstract(AppContext appContext, MasterBuilder masterBuilder)
//        {
//            this.appContext = appContext;
//            this.masterBuilder = masterBuilder;
//        }

//        #endregion

//        #region OnActionExecuted filter

//        /// <summary>
//        /// This filter is used to populate global data for MasterView
//        /// </summary>
//        /// <param name="filterContext"></param>
//        protected override void OnActionExecuted(ActionExecutedContext filterContext)
//        {
//            base.OnActionExecuted(filterContext);

//            var model = filterContext.Controller.ViewData.Model as MasterView;

//            if (model != null)
//            {
//                model.Master = this.masterBuilder.GetMasterModel();
//            }
//        }

//        #endregion

//        #region Dispose

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                appContext.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #endregion
//    }
//}
