using Core.Helpers.Privilege;
using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.System;
using UI.Events;
using Web.Lib.Privilege;

namespace Web.Controllers.System
{

    [AuthorizeRolesMvc(PrivilegeLevel.Admin)]
    public class SystemController : BaseController
    {

        #region Builder

        private readonly SystemBuilder _systemBuilder;

        #endregion

        #region Constructor

        public SystemController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, SystemBuilder systemBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _systemBuilder = systemBuilder;
        }

        #endregion

        [Route("System/EventLog")]
        public async Task<ActionResult> EventLog(int? page)
        {
            var model = await _systemBuilder.BuildEventLogViewAsync(page);

            if (model == null)
            {
                return HttpNotFound();
            }

            // mark latest read event log
            var latestLog = model.Events.OrderByDescending(m => m.ID).FirstOrDefault();
            if (latestLog  != null)
            {
                _systemBuilder.MarkReadLog(latestLog.ID);
            }

            return View(model);
        }

        [Route("System/EmailLog")]
        public async Task<ActionResult> EmailLog(int? page, bool? onlyunsent)
        {
            var model = await _systemBuilder.BuildEmailLogViewAsync(page, onlyunsent ?? false);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }
    }
}