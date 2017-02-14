using System;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Events;
using UI.Modules.CompanyGallery;
using UI.Modules.CompanyGallery.Models;

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