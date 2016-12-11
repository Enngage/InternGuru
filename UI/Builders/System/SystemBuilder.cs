using PagedList;
using System.Linq;
using PagedList.EntityFramework;
using System.Threading.Tasks;
using UI.Base;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Builders.System.Models;
using UI.Builders.System.Views;
using UI.Builders.Auth.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using Core.Config;

namespace UI.Builders.Company
{
    public class SystemBuilder : BaseBuilder
    {

        #region Variables

        public readonly string LATEST_READ_LOG_COOKIE_NAME = AppConfig.CookieNames.LatestReadLogID;

        #endregion

        #region Auth builder

        private AuthBuilder authBuilder;

        #endregion

        #region Constructor

        public SystemBuilder(ISystemContext systemContext, IServicesLoader servicesLoader, AuthBuilder authBuilder) : base(systemContext, servicesLoader)
        {
            this.authBuilder = authBuilder;
        }

        #endregion

        #region Actions

        public async Task<SystemEventLogView> BuildEventLogViewAsync(int? page)
        {
            int pageSize = 30;

            return new SystemEventLogView()
            {
                AuthMaster = await GetAuthMaster(),
                Events = await GetEvents(page, pageSize)
            };
        }

        public void MarkReadLog(int idOfLatestLog)
        {
            this.Services.CookieService.SetCookie(LATEST_READ_LOG_COOKIE_NAME, idOfLatestLog.ToString(), DateTime.Now.AddMonths(1));
        }

        #endregion

        #region Helper methods

        private async Task<AuthMasterModel> GetAuthMaster()
        {
            return await this.authBuilder.GetAuthMasterModelAsync();
        }

        private async Task<IPagedList<SystemEventModel>> GetEvents(int? page, int pageSize)
        {
            int pageNumber = (page ?? 1);

            var eventsQuery = this.Services.LogService.GetAll()
                .Select(m => new SystemEventModel()
                {
                    ApplicationUserName = m.ApplicationUserName,
                    Created = m.Created,
                    ExceptionMessage = m.ExceptionMessage,
                    ID = m.ID,
                    InnerException = m.InnerException,
                    Stacktrace = m.Stacktrace,
                    Url = m.Url
                })
                .OrderByDescending(m => m.ID);

            return await eventsQuery.ToPagedListAsync(pageNumber, pageSize);
        }

        #endregion

    }
}
