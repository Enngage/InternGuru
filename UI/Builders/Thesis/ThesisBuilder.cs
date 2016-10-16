using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using UI.Base;
using Core.Context;
using UI.Builders.Services;
using UI.Builders.Thesis.Views;
using UI.Builders.Thesis.Models;
using Entity;
using System.Data.Entity;

namespace UI.Builders.Thesis
{
    public class ThesisBuilder : BaseBuilder
    {

        #region Constructor

        public ThesisBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<ThesisDetailView> BuildDetailViewAsync(int thesisID)
        {
            var thesis = await GetThesisDetailModelAsync(thesisID);

            if (thesis == null)
            {
                return null;
            }

            return new ThesisDetailView()
            {
                Thesis = thesis
            };
        }

        #endregion

        #region Helper methods


        /// <summary>
        /// Gets thesis model
        /// </summary>
        /// <param name="thesisID">ThesisID</param>/param>
        /// <returns>Thesis or null if none is found</returns>
        private async Task<ThesisDetailModel> GetThesisDetailModelAsync(int thesisID)
        {
            var thesisQuery = this.Services.ThesisService.GetSingle(thesisID)
                .Where(m => m.IsActive == true)
                .Select(m => new ThesisDetailModel()
                {
                    Company = new ThesisDetailCompanyModel()
                    {
                        CompanyCodeName = m.Company.CodeName,
                        CompanyID = m.CompanyID,
                        CompanyName = m.Company.CompanyName,
                        Lat = m.Company.Lat,
                        Lng = m.Company.Lng,
                        Address = m.Company.Address,
                        City = m.Company.City,
                        CompanySizeName = m.Company.CompanySize.CompanySizeName,
                        CountryName = m.Company.Country.CountryName,
                        CountryCode = m.Company.Country.CountryCode,
                        Facebook = m.Company.Facebook,
                        LinkedIn = m.Company.LinkedIn,
                        LongDescription = m.Company.LongDescription,
                        PublicEmail = m.Company.PublicEmail,
                        Twitter = m.Company.Twitter,
                        Web = m.Company.Web,
                        YearFounded = m.Company.YearFounded
                    },
                    Amount = m.Amount,
                    CodeName = m.CodeName,
                    CurrencyName = m.Currency.CurrencyName,
                    CurrencyShowSignOnLeft = m.Currency.ShowSignOnLeft,
                    Created = m.Created,
                    Description = m.Description,
                    ID = m.ID,
                    IsPaid = m.IsPaid,
                    CurrencyID = m.CurrencyID,
                    InternshipCategoryID = m.InternshipCategoryID,
                    InternshipCategoryName = m.InternshipCategory.Name,
                    ThesisName = m.ThesisName,
                    ThesisTypeID = m.ThesisTypeID,
                    ThesisTypeName = m.ThesisType.Name,
                    ThesisTypeCodeName = m.ThesisType.CodeName
                });

            int cacheMinutes = 120;
            var cacheSetup = this.Services.CacheService.GetSetup<ThesisDetailModel>(this.GetSource(), cacheMinutes);

            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID)
            };
            cacheSetup.ObjectID = thesisID;

            var thesis = await this.Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);

            // set thesis type value
            thesis.ThesisTypeNameConverted = thesis.ThesisTypeCodeName.Equals("all") ? string.Join("/", await GetAllThesisTypesAsync()) : thesis.ThesisTypeName;

            return thesis;

        }

        /// <summary>
        /// Gets all thesis types
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await this.Services.ThesisTypeService.GetAllCachedAsync())
                .Select(m => m.Name)
                .ToList();
        }

        #endregion
    }
}
