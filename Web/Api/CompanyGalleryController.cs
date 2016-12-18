

using Service.Context;
using System;
using System.Web.Http;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.CompanyGallery.Models;
using UI.Builders.Master;
using UI.Events;

namespace Web.Api.Controllers
{
    public class CompanyGalleryController : BaseApiController
    {
        CompanyGalleryBuilder companyGalleryBuilder;

        public CompanyGalleryController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyGalleryBuilder companyGalleryBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            this.companyGalleryBuilder = companyGalleryBuilder;
        }

        #region Actions

        [HttpPost]
        public IHttpActionResult DeleteImage(CompanyGalleryDeleteFileQuery query)
        {
            try
            {
                companyGalleryBuilder.DeleteImage(query.CompanyGuid, query.FileName);

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