﻿using System;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.InfoMessage;
using UI.Builders.InfoMessage.Models;
using UI.Builders.Master;
using UI.Events;

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
                _infoMessageBuilder.ProcessClosableMessage(query.MessageID, query.ClosedUntil, query.RememberClosed);

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