using Core.Context;
using UI.Builders.Master;
using UI.ModelState;
using System.Web.Http;

namespace UI.Abstract
{
    public abstract class BaseApiController: ApiController
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

        #endregion

        #region Constructors

        public BaseApiController(IAppContext appContext, MasterBuilder masterBuilder)
        {
            this.appContext = appContext;
            this.masterBuilder = masterBuilder;
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
