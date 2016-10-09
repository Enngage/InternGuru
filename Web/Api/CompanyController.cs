using Common.Helpers;
using Core.Context;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Enums;
using UI.Builders.Company.Models;
using UI.Builders.Master;
using UI.Helpers;

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
                var companies = await companyBuilder.GetMoreCompaniesAsync(query.PageNumber, query.Search);

                // add company URLs and banner image URLs
                foreach (var company in companies)
                {
                    company.Url = this.Url.Link("Company", new { codeName = company.CodeName });
                    company.UrlToInternships = this.Url.Link("Company", new { codeName = company.CodeName, tab = CompanyDetailMenuEnum.Internships });
                    company.LogoImageUrl = ImageHelper.GetCompanyLogo(company.ID);
                    company.BannerImageUrl = ImageHelper.GetCompanyBanner(company.ID);
                    company.CountryIcon = CountryHelper.GetCountryIcon(company.CountryCode);
                    company.PluralInternshipsCountWord = StringHelper.GetPluralWord(company.InternshipCount, "nabídka", "nabídky", "nabídek");
                }

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