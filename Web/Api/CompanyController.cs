using Core.Context;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Models;
using UI.Builders.Master;

namespace Web.Api.Controllers
{
    public class CompanyController : BaseApiController
    {
        CompanyBuilder companyBuilder;

        public CompanyController(IAppContext appContext, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, masterBuilder)
        {
            this.companyBuilder = companyBuilder;
        }

        #region Actions

        [HttpPost]
        public async Task<IHttpActionResult> GetMoreCompanies(GetMoreCompaniesModel query)
        {
            try
            {
                var companies = await companyBuilder.GetMoreCompaniesAsync(1);

                System.Threading.Thread.Sleep(5000);

                return Ok(companies);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}