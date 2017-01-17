using System;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Helpers;
using Service.Context;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Enums;
using UI.Builders.Company.Models;
using UI.Builders.Master;
using UI.Events;
using UI.Helpers;

namespace Web.Api
{
    public class CompanyController : BaseApiController
    {
        readonly CompanyBuilder _companyBuilder;

        public CompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _companyBuilder = companyBuilder;
        }

        #region Actions

        [HttpPost]
        public async Task<IHttpActionResult> GetMoreCompanies(GetMoreCompaniesModel query)
        {
            try
            {
                var companies = await _companyBuilder.GetMoreCompaniesAsync(query.PageNumber, query.Search);

                // add company URLs and banner image URLs
                foreach (var company in companies)
                {
                    company.Url = Url.Link("Company", new { codeName = company.CodeName });
                    company.UrlToInternships = Url.Link("Company", new { codeName = company.CodeName, tab = CompanyDetailMenuEnum.Internships });
                    company.UrlToTheses = Url.Link("Company", new { codeName = company.CodeName, tab = CompanyDetailMenuEnum.Theses });
                    company.LogoImageUrl = ImageHelper.GetCompanyLogoStatic(company.CompanyGuid);
                    company.BannerImageUrl = ImageHelper.GetCompanyBannerStatic(company.CompanyGuid);
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