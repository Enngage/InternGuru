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
                        Description = $"{CountryHelper.GetCountryIcon(m.CountryCode)} | {m.InternshipCount} {StringHelper.GetPluralWord(m.InternshipCount, "nabídka", "nabídky", "nabídek")}",
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

        #endregion
    }
}