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

        /// <summary>
        /// Gets all internship keywords (title of internship split by words)
        /// </summary>
        /// <param name="search">Search value</param>
        /// <returns>Distinct internship title keywords</returns>
        public async Task<IEnumerable<SearchInternshipTitleModel>> GetInternshipTitleKeywords(string search)
        {
            var cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<SearchInternshipTitleModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var internshipQuery = this.Services.InternshipService.GetAll()
                .GroupBy(m => new
                {
                    Title = m.Title,
                })
                .Select(m => new
                {
                    Title = m.Key.Title,
                    InternshipCount = m.Count()
                });

           
            var internshipsKeywords = await this.Services.CacheService.GetOrSet(async () => await internshipQuery.ToListAsync(), cacheSetup);

            // split title of all internship
            var keywordList = new List<SearchInternshipTitleModel>();

            foreach (var internship in internshipsKeywords.Where(m => m.Title.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                var keywords = internship.Title.Split(' ');

                foreach(var keyword in keywords)
                {
                    keywordList.Add(new SearchInternshipTitleModel()
                    {
                        InternshipCount = internship.InternshipCount,
                        TitleKeyword = keyword.Trim()
                    });
                }
            }

            return keywordList;
        }

        /// <summary>
        /// Gets all thesis keywords (Name of thesis split by words)
        /// </summary>
        /// <param name="search">Search value</param>
        /// <returns>Distinct internship title keywords</returns>
        public async Task<IEnumerable<SearchThesisKeywordModel>> GetThesisNameKeywords(string search)
        {
            var cacheMinutes = 60;
            var cacheSetup = this.Services.CacheService.GetSetup<SearchThesisKeywordModel>(this.GetSource(), cacheMinutes);
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>()
            };

            var thesisQuery = this.Services.ThesisService.GetAll()
                .GroupBy(m => new
                {
                    ThesisName = m.ThesisName,
                })
                .Select(m => new
                {
                    ThesisName = m.Key.ThesisName,
                    ThesisCount = m.Count()
                });


            var thesisNames = await this.Services.CacheService.GetOrSet(async () => await thesisQuery.ToListAsync(), cacheSetup);

            // split name
            var keywordList = new List<SearchThesisKeywordModel>();

            foreach (var thesis in thesisNames.Where(m => m.ThesisName.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                var keywords = thesis.ThesisName.Split(' ');

                foreach (var keyword in keywords)
                {
                    keywordList.Add(new SearchThesisKeywordModel()
                    {
                        ThesisCount = thesis.ThesisCount,
                        ThesisKeyword = keyword.Trim()
                    });
                }
            }

            return keywordList;
        }

        #endregion
    }
}
