using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System;

using Core.Extensions;
using Entity.Base;
using UI.Base;
using UI.Builders.Search.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Search
{
    public class SearchBuilder : BaseBuilder
    {

        #region Variables / Config

        /// <summary>
        /// Array of characters used for splitting text
        /// </summary>
        private static string[] KeyWordSplitCharacters => new [] { " ", "/", "-", "\\", "_", "[", "]", ".", "?", "!" };

        #endregion

        #region Constructor

        public SearchBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Methods

        /// <summary>
        /// Gets cities that are currently used in Internship along with the number of available internships in that city
        /// </summary>
        /// <param name="searchForCities">Cities that should be searched</param>
        /// <returns>Cities with the number of available internships</returns>
        public async Task<IEnumerable<SearchCityModel>> GetSearchCitiesAsync(string searchForCities)
        {
            var cacheSetup = Services.CacheService.GetSetup<SearchCityModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var citiesQuery = Services.InternshipService.GetAll()
                .GroupBy(m => new
                {
                    m.City,
                    CountryCode = m.Country.CodeName
                })
                .Select(m => new SearchCityModel()
                {
                    City = m.Key.City,
                    CountryCode = m.Key.CountryCode,
                    InternshipCount = m.Count()
                });

            // load and cache all cities. Search in memory by ling
            var allCities = await Services.CacheService.GetOrSet(async () => await citiesQuery.ToListAsync(), cacheSetup);

            return allCities.Where(m => m.City.Contains(searchForCities.Trim(), StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all internship keywords (title of internship split by words)
        /// </summary>
        /// <param name="search">Search value</param>
        /// <returns>Distinct internship title keywords</returns>
        public async Task<IEnumerable<SearchInternshipTitleModel>> GetInternshipTitleKeywords(string search)
        {
            var cacheSetup = Services.CacheService.GetSetup<SearchInternshipTitleModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyUpdateAny<Entity.Internship>()
            };

            var internshipQuery = Services.InternshipService.GetAll()
                .GroupBy(m => new
                {
                    m.Title,
                })
                .Select(m => new
                {
                    m.Key.Title,
                    InternshipCount = m.Count()
                });


            var internshipsKeywords = await Services.CacheService.GetOrSet(async () => await internshipQuery.ToListAsync(), cacheSetup);

            // split title of all internship
            var keywordList = new List<SearchInternshipTitleModel>();

            foreach (var internship in internshipsKeywords.Where(m => m.Title.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                var keywords = internship.Title.Split(KeyWordSplitCharacters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var keyword in keywords)
                {

                    var existingKeyword = keywordList.Where(m => m.TitleKeyword == keyword).FirstOrDefault();
                    if (existingKeyword != null)
                    {
                        // increase number of occurences
                        existingKeyword.InternshipCount++;

                        continue;
                    }

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
            var cacheSetup = Services.CacheService.GetSetup<SearchThesisKeywordModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyUpdateAny<Entity.Thesis>()
            };

            var thesisQuery = Services.ThesisService.GetAll()
                .GroupBy(m => new
                {
                    m.ThesisName,
                })
                .Select(m => new
                {
                    m.Key.ThesisName,
                    ThesisCount = m.Count()
                });


            var thesisNames = await Services.CacheService.GetOrSet(async () => await thesisQuery.ToListAsync(), cacheSetup);

            // split name
            var keywordList = new List<SearchThesisKeywordModel>();

            foreach (var thesis in thesisNames.Where(m => m.ThesisName.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                var keywords = thesis.ThesisName.Split(KeyWordSplitCharacters, StringSplitOptions.RemoveEmptyEntries);

                foreach (var keyword in keywords)
                {

                    var existingKeyword = keywordList.Where(m => m.ThesisKeyword == keyword).FirstOrDefault();
                    if (existingKeyword != null)
                    {
                        // increase number of occurences
                        existingKeyword.ThesisCount++;

                        continue;
                    }

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
