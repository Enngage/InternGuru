﻿using System;
using Service.Context;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.FineUpload;
using UI.Builders.FineUpload.Models;
using UI.Builders.Master;
using UI.Events;

namespace Web.Controllers
{
    public class UploadController : BaseController
    {

        private readonly FineUploadBuilder _fineUploadBuilder;

        public UploadController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, FineUploadBuilder fineUploadBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _fineUploadBuilder = fineUploadBuilder;
        }

        #region Actions

        [Route("Uploader/CompanyGallery")]
        [HttpPost]
        public FineUploaderResult CompanyGallery(FineUploadModel upload)
        {
            try
            {
                _fineUploadBuilder.UploadCompanyGalleryFile(upload);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true);
        }


        [Route("Uploader/Avatar")]
        [HttpPost]
        public FineUploaderResult Avatar(FineUploadModel upload)
        {
            try
            {
                _fineUploadBuilder.UploadAvatar(upload);
            }
            catch (Exception ex)
            {
                return new FineUploaderResult(false, error: ex.Message);
            }

            // the anonymous object in the result below will be convert to json and set back to the browser
            return new FineUploaderResult(true);
        }


        #endregion

    }
}