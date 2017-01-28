using System;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Events;
using UI.Modules.InfoMessage;
using UI.Modules.InfoMessage.Models;

namespace Web.Api
{
    public class InfoMessageController : BaseApiController
    {
        readonly InfoMessageBuilder _infoMessageBuilder;

        public InfoMessageController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, InfoMessageBuilder infoMessageBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            this._infoMessageBuilder = infoMessageBuilder;
        }

        #region Actions

        [HttpPost]
        public IHttpActionResult ProcessClosableMessage(InfoMessageProcessClosableMessageQuery query)
        {
            try
            {
                _infoMessageBuilder.ProcessClosableMessage(query.MessageID, query.ClosedForDaysCount, query.RememberClosed);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}