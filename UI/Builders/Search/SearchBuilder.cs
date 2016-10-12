using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

using Common.Extensions;
using Core.Context;
using Entity;
using UI.Base;
using UI.Builders.Services;

namespace UI.Builders.Search
{
    public class SearchBuilder : BaseBuilder
    {

        #region Constructor

        public SearchBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets cities that are currently used in Internship along with the number of available internships in that city
        /// </summary>
        /// <param name="searchForCities">Cities that should be searched</param>
        /// <returns>Cities with the number of available internships</returns>
        public async Task<IEnumerable<SearchCityModel>> GetSearchCitiesAsync(string searchForCities)
        {
            var cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<SearchCityModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var citiesQuery = this.Services.InternshipService.GetAll()
                .GroupBy(m => new
                {
                    City = m.City,
                    CountryCode = m.Country.CodeName
                })
                .Select(m => new SearchCityModel()
                {
                    City = m.Key.City,
                    CountryCode = m.Key.CountryCode,
                    InternshipCount = m.Count()
                });

            // load and cache all cities. Search in memory by ling
            var allCities = await this.Services.CacheService.GetOrSet(async () => await citiesQuery.ToListAsync(), cacheSetup);

            return allCities.Where(m => m.City.Contains(searchForCities.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        #endregion
    }
}
