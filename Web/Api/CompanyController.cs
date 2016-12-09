using Core.Helpers;
using Service.Context;
using System;
using System.Threading.Tasks;
using System.Web.Http;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Enums;
using UI.Builders.Company.Models;
using UI.Builders.Master;
using UI.Events;
using UI.Helpers;

namespace Web.Api.Controllers
{
    public class CompanyController : BaseApiController
    {
        CompanyBuilder companyBuilder;

        public CompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, serviceEvents, masterBuilder)
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
                    company.UrlToTheses = this.Url.Link("Company", new { codeName = company.CodeName, tab = CompanyDetailMenuEnum.Theses });
                    company.LogoImageUrl = ImageHelper.GetCompanyLogo(company.ID);
                    company.BannerImageUrl = ImageHelper.GetCompanyBanner(company.ID);
                    company.PluralInternshipsCountWord = StringHelper.GetPluralWord(company.InternshipCount, "žádné volné stáže", "{count} stáž", "{count} stáže", "{count} stáže");
                    company.PluralThesesCountWord = StringHelper.GetPluralWord(company.ThesesCount, "nenabízí závěrečné práce", "{count} závěrečná práce", "{count} závěrečné práce", "{count} závěrečných prací");
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