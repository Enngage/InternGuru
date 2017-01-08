using UI.Base;
using UI.Builders.Services;
using System;
using UI.Builders.Shared.Models;

namespace UI.Builders.InfoMessage
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
        public void ProcessClosableMessage(string messageID, DateTime closedUntil, bool rememberClosed)
        {
            if (rememberClosed)
            {
                this.Services.CookieService.SetCookie(GetClosedMessageCookieName(messageID), "1", closedUntil);
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
