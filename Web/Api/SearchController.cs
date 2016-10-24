using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Linq;

using Core.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Search;
using Common.Helpers;
using UI.Helpers;

namespace Web.Api.Controllers
{
    public class SearchController : BaseApiController
    {
        SearchBuilder searchBuilder;

        public SearchController(IAppContext appContext, MasterBuilder masterBuilder, SearchBuilder searchBuilder) : base (appContext, masterBuilder)
        {
            this.searchBuilder = searchBuilder;
        }

        #region Actions

        [HttpGet]
        public async Task<IHttpActionResult> GetSearchCities([FromUri] SearchQModel query)
        {
            try
            {
                var cities = (await searchBuilder.GetSearchCitiesAsync(query.q))
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{CountryHelper.GetCountryIcon(m.CountryCode)} | {m.InternshipCount} {StringHelper.GetPluralWord(m.InternshipCount, "žádné nabídky", "nabídka", "nabídky", "nabídek")}",
                        Title = m.City.Trim()
                    });

                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = cities.Count(),
                    Items = cities
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
                var keywords = (await searchBuilder.GetThesisNameKeywords(query.q))
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{m.ThesisCount} {StringHelper.GetPluralWord(m.ThesisCount, "žádné práce", "práce", "práce", "prací")}",
                        Title = m.ThesisKeyword.Trim()
                    });

                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = keywords.Count(),
                    Items = keywords
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
                var keywords = (await searchBuilder.GetInternshipTitleKeywords(query.q))
                    .Select(m => new SearchAutocompleteItemModel()
                    {
                        Description = $"{m.InternshipCount} {StringHelper.GetPluralWord(m.InternshipCount, "žádné nabídky", "nabídka", "nabídky", "nabídek")}",
                        Title = m.TitleKeyword.Trim()
                    });

                return Ok(new SearchAutocompleteResultModel()
                {
                    Count = keywords.Count(),
                    Items = keywords
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