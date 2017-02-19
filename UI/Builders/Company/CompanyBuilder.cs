using System.Data.Entity;
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
using Service.Exceptions;
using UI.Builders.Shared.Models;
using PagedList;
using Core.Extensions;
using Entity.Base;
using Service.Services.Activities.Enums;

namespace UI.Builders.Company
{
    public class CompanyBuilder : BaseBuilder
    {

        #region Variables

        private readonly int _browseCompaniesPageSize = 4;

        #endregion

        #region Constructor

        public CompanyBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<CompanyBrowseView> BuildBrowseViewAsync(int? page)
        {
            var pageNumber = (page ?? 1);

            return new CompanyBrowseView()
            {
                Companies = await GetCompaniesAsync(pageNumber, null)
            };
        }

        public async Task<CompanyDetailView> BuildDetailViewAsync(string codeName, CompanyContactUsForm contactUsForm = null)
        {
            var cacheSetup = Services.CacheService.GetSetup<CompanyDetailModel>(GetSource());
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
                    CodeName = m.CodeName,
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
                    CompanyGuid = m.Guid,
                    ID = m.ID,
                    Internships = m.Internships
                        .Where(s => s.IsActive)
                        .Select(s => new CompanyDetailInternshipModel()
                        {
                            ID = s.ID,
                            Title = s.Title,
                            Amount = s.Amount,
                            AmountTypeName = s.AmountType.AmountTypeName,
                            IsPaid = s.IsPaid,
                            HideAmount = s.HideAmount,
                            CurrencyName = s.Currency.CurrencyName,
                            CurrencyCode = s.Currency.CodeName,
                            CodeName = s.CodeName,
                            CurrencyShowSignOnLeft = s.Currency.ShowSignOnLeft
                        }),
                    Theses = m.Theses
                        .Where(s => s.IsActive)
                        .Select(s => new CompanyThesisModel()
                        {
                            Amount = s.Amount,
                            CodeName = s.CodeName,
                            CurrencyName = s.Currency.CurrencyName,
                            ID = s.ID,
                            IsPaid = s.IsPaid,
                            HideAmount = s.HideAmount,
                            Name = s.ThesisName,
                            CurrencyCodeName = s.Currency.CodeName,
                            CurrencyDisplaySignOnLeft = s.Currency.ShowSignOnLeft
                        })
                });

            var company = await Services.CacheService.GetOrSetAsync(async () => await companyQuery.FirstOrDefaultAsync(), cacheSetup);

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
                ContactUsForm = contactUsForm ?? defaultForm,
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Logs activity for viewing company profile
        /// </summary>
        /// <param name="companyID">companyID</param>
        public async Task<int> LogCompanyProfileViewActivityAsync(int companyID)
        {
            if (companyID == 0)
            {
                return 0;
            }

            var activityUserId = CurrentUser.IsAuthenticated ? CurrentUser.Id : null;

            var result = await Services.ActivityService.LogActivity(ActivityTypeEnum.ViewCompanyProfile, companyID, activityUserId, companyID);

            return result.ObjectID;
        }

        public async Task<int> CreateMessage(CompanyContactUsForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send message
                    throw new ValidationException("Pro odeslání zprávy se prosím přihlašte");
                }

                // get recipient (company's representative)
                var companyUserID = await GetIDOfCompanyUserAsync(form.CompanyID);

                if (string.IsNullOrEmpty(companyUserID))
                {
                    throw new ValidationException("Firma neexistuje");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = CurrentUser.Id,
                    RecipientCompanyID = form.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = null, // no subject needed
                    IsRead = false,
                };

                var result = await Services.MessageService.InsertAsync(message);

                return result.ObjectID;
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
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
            return await Services.CompanyService.GetSingle(companyID)
                .Select(m => m.CreatedByApplicationUserId)
                .FirstOrDefaultAsync();
        }
 
        private async Task<IList<CompanyBrowseModel>> GetCompaniesAsync(int pageNumber, string search)
        {
            // get companies from db/cache
            var cacheSetup = Services.CacheService.GetSetup<CompanyBrowseModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Company>(),
                EntityKeys.KeyDeleteAny<Entity.Company>(),
                EntityKeys.KeyUpdateAny<Entity.Company>()
            };

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
                    CompanyGuid = m.Guid,
                    InternshipCount = m.Internships.Where(s => s.IsActive).Count(),
                    CodeName = m.CodeName,
                    ThesesCount = m.Theses.Count()
                });

            // cache all companies
            var allCompanies = await Services.CacheService.GetOrSetAsync(async () => await companiesQuery.ToListAsync(), cacheSetup);

            // search
            if (!string.IsNullOrEmpty(search))
            {
                var filteredCompanies = allCompanies.Where(m => m.CompanyName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToPagedList(pageNumber, _browseCompaniesPageSize);

                return filteredCompanies.ToList();
            }
            // not search
            else
            {
                return allCompanies.ToPagedList(pageNumber, _browseCompaniesPageSize).ToList();
            }
        }

        #endregion
    }
}
