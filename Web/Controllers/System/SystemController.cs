using Core.Helpers.Privilege;
using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Master;
using UI.Events;
using Web.Lib.Authorize;

namespace Web.Controllers.System
{

    [AuthorizeRolesMVC(PrivilegeLevel.Admin)]
    public class SystemController : BaseController
    {

        #region Builder

        private SystemBuilder systemBuilder;

        #endregion

        #region Constructor

        public SystemController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, SystemBuilder systemBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            this.systemBuilder = systemBuilder;
        }

        #endregion

        public async Task<ActionResult> EventLog(int? page)
        {
            var model = await this.systemBuilder.BuildEventLogViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            // mark latest read event log
            var latestLog = model.Events.OrderByDescending(m => m.ID).FirstOrDefault();
            if (latestLog  != null)
            {
                systemBuilder.MarkReadLog(latestLog.ID);
            }

            return View(model);
        }
    }
}