using System;
using System.Threading.Tasks;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.Master;
using UI.Builders.Thesis;
using UI.Builders.Thesis.Data;
using UI.Events;

namespace Web.Api
{
    public class ThesisController : BaseApiController
    {
        readonly ThesisBuilder _thesisBuilder;

        public ThesisController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, ThesisBuilder thesisBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _thesisBuilder = thesisBuilder;
        }

        #region Actions

        [HttpPost]
        public async Task<IHttpActionResult> DeleteThesis(ThesisDeleteThesisData query)
        {
            try
            {
                await _thesisBuilder.DeleteThesisAsync(query.ThesisID);

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        #endregion
    }
}