using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using PagedList;
using PagedList.EntityFramework;
using UI.Base;
using UI.Builders.Auth;
using UI.Builders.Auth.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Builders.System.Models;
using UI.Builders.System.Views;

namespace UI.Builders.System
{
    public class SystemBuilder : BaseBuilder
    {

        #region Variables

        public readonly string LatestReadLogCookieName = AppConfig.CookieNames.LatestReadLogID;

        #endregion

        #region Auth builder

        private readonly AuthBuilder _authBuilder;

        #endregion

        #region Constructor

        public SystemBuilder(ISystemContext systemContext, IServicesLoader servicesLoader, AuthBuilder authBuilder) : base(systemContext, servicesLoader)
        {
            _authBuilder = authBuilder;
        }

        #endregion

        #region Actions

        public async Task<SystemEventLogView> BuildEventLogViewAsync(int? page)
        {
            const int pageSize = 30;

            return new SystemEventLogView()
            {
                AuthMaster = await GetAuthMaster(),
                Events = await GetEvents(page, pageSize)
            };
        }


        public async Task<SystemEmailLogView> BuildEmailLogViewAsync(int? page, bool showOnlyUnsentEmails)
        {
            const int pageSize = 30;

            return new SystemEmailLogView()
            {
                AuthMaster = await GetAuthMaster(),
                Emails = await GetEmails(page, pageSize, showOnlyUnsentEmails)
            };
        }

        public void MarkReadLog(int idOfLatestLog)
        {
            Services.CookieService.SetCookie(LatestReadLogCookieName, idOfLatestLog.ToString(), DateTime.Now.AddMonths(1));
        }

        #endregion

        #region Helper methods

        private async Task<AuthMasterModel> GetAuthMaster()
        {
            return await _authBuilder.GetAuthMasterModelAsync();
        }

        private async Task<IPagedList<SystemEventModel>> GetEvents(int? page, int pageSize)
        {
            var pageNumber = (page ?? 1);

            var eventsQuery = Services.LogService.GetAll()
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

        private async Task<IPagedList<SystemEmailModel>> GetEmails(int? page, int pageSize, bool showOnlyUnsentEmails)
        {
            var pageNumber = (page ?? 1);

            var emailsQuery = Services.EmailService.GetAll()
                .Select(m => new SystemEmailModel()
                {
                    Created = m.Created,
                    ID = m.ID,
                    To = m.To,
                    Subject = m.Subject,
                    HtmlBody = m.HtmlBody,
                    From = m.From,
                    Result = m.Result,
                    IsSent = m.IsSent,
                    Sent = m.Sent
                });

            if (showOnlyUnsentEmails)
            {
                emailsQuery = emailsQuery.Where(m => !m.IsSent);
            }

            emailsQuery = emailsQuery.OrderByDescending(m => m.ID);

            return await emailsQuery.ToPagedListAsync(pageNumber, pageSize);
        }

        #endregion

    }
}
