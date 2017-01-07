﻿using System;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.CompanyGallery;
using UI.Builders.CompanyGallery.Models;
using UI.Builders.Master;
using UI.Events;

namespace Web.Api
{
    public class CompanyGalleryController : BaseApiController
    {
        readonly CompanyGalleryBuilder _companyGalleryBuilder;

        public CompanyGalleryController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyGalleryBuilder companyGalleryBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _companyGalleryBuilder = companyGalleryBuilder;
        }

        #region Actions

        [HttpPost]
        public IHttpActionResult DeleteImage(CompanyGalleryDeleteFileQuery query)
        {
            try
            {
                _companyGalleryBuilder.DeleteImage(query.CompanyGuid, query.FileName);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}