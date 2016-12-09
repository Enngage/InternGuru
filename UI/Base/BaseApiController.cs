using Service.Context;
using UI.Builders.Master;
using UI.ModelState;
using System.Web.Http;
using UI.Events;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace UI.Base
{
    public abstract class BaseApiController: ApiController
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
        /// ServiceEvents 
        /// </summary>
        public IServiceEvents ServiceEvents
        {
            get
            {
                return this.serviceEvents;
            }
        }

        #endregion

        #region Constructors

        public BaseApiController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder)
        {
            this.appContext = appContext;
            this.masterBuilder = masterBuilder;
            this.serviceEvents = serviceEvents;
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

        #region Filters

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            // register service events
            this.ServiceEvents.RegisterEvents();

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        #endregion
    }
}
