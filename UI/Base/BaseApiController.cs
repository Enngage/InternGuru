using Service.Context;
using System.Web.Http;
using UI.Events;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using UI.Builders.Master;

namespace UI.Base
{
    public abstract class BaseApiController: ApiController
    {
        #region Master builder

        /// <summary>
        /// Master Builder 
        /// </summary>
        public MasterBuilder MasterBuilder { get; }

        #endregion

        #region Properties

        /// <summary>
        /// AppContext 
        /// </summary>
        public IAppContext AppContext { get; }

        /// <summary>
        /// ServiceEvents 
        /// </summary>
        public IServiceEvents ServiceEvents { get; }

        #endregion

        #region Constructors

        protected BaseApiController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder)
        {
            AppContext = appContext;
            ServiceEvents = serviceEvents;
            MasterBuilder = masterBuilder;
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

        #region Filters

        public override Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            // register service events
            ServiceEvents.RegisterEvents();

            return base.ExecuteAsync(controllerContext, cancellationToken);
        }

        #endregion
    }
}
