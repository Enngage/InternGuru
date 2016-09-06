using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

using UI.Abstract;
using Cache;
using Core.Context;
using Core.Services;
using UI.Builders.Auth.Views;
using UI.Builders.Auth.Forms;
using System;

namespace UI.Builders.Company
{
    public class AuthBuilder : BuilderAbstract
    {
        #region Services

        ICompanyService companyService;

        #endregion

        #region Constructor

        public AuthBuilder(
            IAppContext appContext,
            ICacheService cacheService,
            ICompanyService companyService,
            IIdentityService identityService,
            ILogService logService)
            : base(
                appContext,
                cacheService,
                identityService,
                logService)
        {
            this.companyService = companyService;
        }

        #endregion

        #region Actions

        public async Task<AuthIndexView> BuildIndexViewAsync()
        {
            var currentUserId = this.CurrentUser.Id;

            // check if user has created company
            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == currentUserId)
                .Take(1)
                .Select(m => new
                {
                    ID = m.ID
                })
                .FirstOrDefaultAsync();

            return new AuthIndexView()
            {
                CompanyIsCreated = company != null
            };
        }

        public AuthRegisterCompanyView BuildRegisterCompanyView(AuthAddEditCompanyForm form)
        {
            if (form == null)
            {
                // user haven't created any company yet
                form = new AuthAddEditCompanyForm();
            }

            // add countries and company sizes
            form.Countries = Common.Helpers.CountryHelper.GetCountries();
            form.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();

            return new AuthRegisterCompanyView()
            {
                CompanyForm = form,
                CompanyIsCreated = form != null
            };
        }

        public async Task<AuthRegisterCompanyView> BuildRegisterCompanyViewAsync()
        {
            var currentUserId = this.CurrentUser.Id;

            // get company assigned to user
            var company = await companyService.GetAll()
                .Where(m => m.ApplicationUserId == currentUserId)
                .Take(1)
                .Select(m => new AuthAddEditCompanyForm()
                {
                    Address = m.Address,
                    City = m.City,
                    CompanyName = m.CompanyName,
                    CompanySize = m.CompanySize,
                    Country = m.Country,
                    Facebook = m.Facebook,
                    ID = m.ID,
                    Lat = m.Lat,
                    Lng = m.Lng,
                    LinkedIn = m.LinkedIn,
                    PublicEmail = m.PublicEmail,
                    LongDescription = m.LongDescription,
                    ShortDescription = m.ShortDescription,
                    Twitter = m.Twitter,
                    Web = m.Web,
                    YearFounded = m.YearFounded
                })
                .FirstOrDefaultAsync();

            if (company == null)
            {
                // user haven't created any company yet
                company = new AuthAddEditCompanyForm();
            }

            // add countries and company sizes
            company.Countries = Common.Helpers.CountryHelper.GetCountries();
            company.AllowedCompanySizes = Common.Helpers.InternshipHelper.GetAllowedCompanySizes();

            return new AuthRegisterCompanyView()
            {
                CompanyForm = company,
                CompanyIsCreated = company != null
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates new company from given form
        /// </summary>
        /// <param name="form">form</param>
        /// <returns>ID of new company</returns>
        public async Task<int> CreateCompany(AuthAddEditCompanyForm form)
        {
            try
            {
                var company = new Entity.Company
                {
                    ApplicationUserId = this.CurrentUser.Id,
                    Address = form.Address,
                    City = form.City,
                    CompanyName = form.CompanyName,
                    CompanySize = form.CompanySize,
                    Country = form.Country,
                    Facebook = form.Facebook,
                    Lat = form.Lat,
                    LinkedIn = form.LinkedIn,
                    Lng = form.Lng,
                    LongDescription = form.LongDescription,
                    PublicEmail = form.PublicEmail,
                    ShortDescription = form.ShortDescription,
                    Twitter = form.Twitter,
                    Web = form.Web,
                    YearFounded = form.YearFounded,
                };

                await companyService.InsertAsync(company);

                return company.ID;
            }
            catch (Exception ex)
            {
                // log error
                LogService.LogException(ex);

                // re-throw
                throw;
            }
        }

        #endregion
    }
}
