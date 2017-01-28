using System;
using UI.Base;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Modules.InfoMessage
{
    public class InfoMessageBuilder : BaseBuilder
    {

        #region Setup

        private const string ClosedMessageCookiePrefix = "cm_";

        #endregion

        #region Constructor

        public InfoMessageBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Web API methods

        /// <summary>
        /// Sets cookie for given message so that it is not displayed until the cookie expires
        /// </summary>
        public void ProcessClosableMessage(string messageID, int closedForDaysCount, bool rememberClosed)
        {
            if (rememberClosed)
            {
                Services.CookieService.SetCookie(GetClosedMessageCookieName(messageID), "1", DateTime.Now.AddDays(closedForDaysCount));
            }
        }

        #endregion

        #region Helper methods

        public static string GetClosedMessageCookieName(string messageID)
        {
            return ClosedMessageCookiePrefix + messageID;
        }

        #endregion

    }
}
