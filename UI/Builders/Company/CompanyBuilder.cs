using Core.Context;
using System.Data.Entity;
using PagedList.EntityFramework;
using System.Linq;
using System.Collections.Generic;
using UI.Base;
using UI.Builders.Company.Models;
using UI.Builders.Company.Views;
using System.Threading.Tasks;
using UI.Builders.Services;
using UI.Builders.Company.Forms;
using System;
using UI.Exceptions;
using Entity;
using System.Collections;

namespace UI.Builders.Company
{
    public class CompanyBuilder : BaseBuilder
    {

        #region Variables

        private readonly int browseCompaniesPageSize = 4;

        #endregion

        #region Constructor

        public CompanyBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<CompanyBrowseView> BuildBrowseViewAsync(int? page)
        {
            int pageNumber = (page ?? 1);

            return new CompanyBrowseView()
            {
                Companies = await GetCompaniesAsync(pageNumber, null)
            };
        }

        public async Task<CompanyDetailView> BuildDetailViewAsync(string codeName, CompanyContactUsForm contactUsForm = null)
        {
            int cacheMinutes = 60;

            var cacheSetup = this.Services.CacheService.GetSetup<CompanyDetailModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateCodeName<Entity.Company>(codeName),
                EntityKeys.KeyDeleteCodeName<Entity.Company>(codeName),
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>()
            };
            cacheSetup.ObjectStringID = codeName;

            var companyQuery = Services.CompanyService.GetAll()
                .Where(m => m.CodeName == codeName)
                .Take(1)
                .Select(m => new CompanyDetailModel()
                {
                    Address = m.Address,
                    CompanyCategoryID = m.CompanyCategoryID,
                    CompanyCategoryName = m.CompanyCategory.Name,
                    CompanySizeID = m.CompanySizeID,
                    CompanySizeName = m.CompanySize.CompanySizeName,
                    CountryCode = m.Country.CountryCode,
                    CountryID = m.CountryID,
                    CountryName = m.Country.CountryName,
                    Facebook = m.Facebook,
                    Lat = m.Lat,
                    LinkedIn = m.LinkedIn,
                    Lng = m.Lng,
                    LongDescription = m.LongDescription,
                    PublicEmail = m.PublicEmail,
                    Twitter = m.Twitter,
                    Web = m.Web,
                    YearFounded = m.YearFounded,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    ID = m.ID,
                    Internships = m.Internships
                        .Where(v => v.IsActive == true)
                        .Select(s => new CompanyDetailInternshipModel()
                        {
                            ID = s.ID,
                            Title = s.Title,
                            Amount = s.Amount,
                            AmountTypeName = s.AmountType.AmountTypeName,
                            IsPaid = s.IsPaid,
                            CurrencyName = s.Currency.CurrencyName,
                            CurrencyCode = s.Currency.CodeName,
                            CodeName = s.CodeName,
                            CurrencyShowSignOnLeft = s.Currency.ShowSignOnLeft
                        }),
                    Theses = m.Theses
                        .Where(v => v.IsActive == true)
                        .Select(s => new CompanyThesisModel()
                        {
                            Amount = s.Amount,
                            CodeName = s.CodeName,
                            CurrencyName = s.Currency.CurrencyName,
                            ID = s.ID,
                            IsPaid = s.IsPaid,
                            Name = s.ThesisName,
                            CurrencyCodeName = s.Currency.CodeName,
                            CurrencyDisplaySignOnLeft = s.Currency.ShowSignOnLeft
                        })
                });

            var company = await this.Services.CacheService.GetOrSetAsync(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

            if (company == null)
            {
                return null;
            }

            var defaultForm = new CompanyContactUsForm()
            {
                Message = string.Empty,
                CompanyCodeName = codeName,
                CompanyID = company.ID
            };

            return new CompanyDetailView()
            {
                Company = company,
                ContactUsForm = contactUsForm == null ? defaultForm : contactUsForm,
            };
        }

        #endregion

        #region Public methods

        public async Task<int> CreateMessage(CompanyContactUsForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send message
                    throw new UIException("Pro odeslání zprávy se prosím přihlašte");
                }

                // get recipient (company's representative)
                var companyUserID = await GetIDOfCompanyUserAsync(form.CompanyID);

                if (string.IsNullOrEmpty(companyUserID))
                {
                    throw new UIException("Firma neexistuje");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = this.CurrentUser.Id,
                    RecipientCompanyID = form.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = null, // no subject needed
                    IsRead = false,
                };

                return await this.Services.MessageService.InsertAsync(message);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
            }
        }

        #endregion

        #region Web API methods

        public Task<IList<CompanyBrowseModel>> GetMoreCompaniesAsync(int pageNumber, string search)
        {
            return GetCompaniesAsync(pageNumber, search);
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets ID of user who created company
        /// </summary>
        /// <param name="companyID">CompanyID</param>
        /// <returns>ID of user who created company</returns>
        private async Task<string> GetIDOfCompanyUserAsync(int companyID)
        {
            return await this.Services.CompanyService.GetSingle(companyID)
                .Select(m => m.ApplicationUserId)
                .FirstOrDefaultAsync();
        }
 
        private async Task<IList<CompanyBrowseModel>> GetCompaniesAsync(int pageNumber, string search)
        {
            int cacheMinutes = 60;

            // get companies from db/cache
            var cacheSetup = this.Services.CacheService.GetSetup<CompanyBrowseModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Company>(),
                EntityKeys.KeyDeleteAny<Entity.Company>(),
                EntityKeys.KeyUpdateAny<Entity.Company>()
            };

            // cache different pages separately
            cacheSetup.PageNumber = pageNumber;

            var companiesQuery = Services.CompanyService.GetAll()
                .OrderBy(m => m.CompanyName)
                .Select(m => new CompanyBrowseModel()
                {
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CountryName = m.Country.CountryName,
                    CountryCode = m.Country.CountryCode,
                    CountryIcon = m.Country.Icon,
                    ID = m.ID,
                    InternshipCount = m.Internships.Count(),
                    CodeName = m.CodeName,
                    ThesesCount = m.Theses.Count()
                });

            // search
            if (!string.IsNullOrEmpty(search))
            {
                companiesQuery = companiesQuery.Where(m => m.CompanyName.Contains(search));

                var companies = await companiesQuery.ToPagedListAsync(pageNumber, browseCompaniesPageSize);

                return companies.ToList();
            }
            // not search
            else
            {
                var companies = await this.Services.CacheService.GetOrSetAsync(async () => await companiesQuery.ToPagedListAsync(pageNumber, browseCompaniesPageSize), cacheSetup);

                return companies.ToList();
            }
        }

        #endregion
    }
}
