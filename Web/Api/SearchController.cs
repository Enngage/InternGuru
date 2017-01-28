using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Helpers;
using Service.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Search;
using UI.Builders.Search.Autocomplete;
using UI.Builders.Search.Models;
using UI.Events;
using UI.Helpers;

namespace Web.Api
{
    public class SearchController : BaseApiController
    {
        readonly SearchBuilder _searchBuilder;

        public SearchController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, SearchBuilder searchBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _searchBuilder = searchBuilder;
        }

        #region Actions

        [HttpGet]
        public async Task<IHttpActionResult> GetSearchCities([FromUri] SearchQModel query)
        {
            try
            {
                var cities = (await _searchBuilder.GetSearchCitiesAsync(query.Q))
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{CountryHelper.GetCountryIconStatic(m.CountryCode)} | {m.InternshipCount} {StringHelper.GetPluralWord(m.InternshipCount, "žádné nabídky", "nabídka", "nabídky", "nabídek")}",
                        Title = m.City.Trim()
                    });

                var searchAutocompleteItemModels = cities as IList<SearchAutocompleteItemModel> ?? cities.ToList();

                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = searchAutocompleteItemModels.Count(),
                    Items = searchAutocompleteItemModels
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetThesisKeywords([FromUri] SearchQModel query)
        {
            try
            {
                var keywords = (await _searchBuilder.GetThesisNameKeywords(query.Q))
                    .OrderByDescending(m => m.ThesisCount)
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{m.ThesisCount} {StringHelper.GetPluralWord(m.ThesisCount, "žádné práce", "práce", "práce", "prací")}",
                        Title = m.ThesisKeyword.Trim()
                    });

                var searchAutocompleteItemModels = keywords as IList<SearchAutocompleteItemModel> ?? keywords.ToList();
                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = searchAutocompleteItemModels.Count(),
                    Items = searchAutocompleteItemModels
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        public async Task<IHttpActionResult> GetInternshipKeywords([FromUri] SearchQModel query)
        {
            try
            {
                var keywords = (await _searchBuilder.GetInternshipTitleKeywords(query.Q))
                    .OrderByDescending(m => m.InternshipCount)
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{m.InternshipCount} {StringHelper.GetPluralWord(m.InternshipCount, "žádné nabídky", "nabídka", "nabídky", "nabídek")}",
                        Title = m.TitleKeyword.Trim()
                    });

                var searchAutocompleteItemModels = keywords as IList<SearchAutocompleteItemModel> ?? keywords.ToList();
                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = searchAutocompleteItemModels.Count(),
                    Items = searchAutocompleteItemModels
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #endregion
    }
}