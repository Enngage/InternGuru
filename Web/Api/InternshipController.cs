using System;
using System.Threading.Tasks;
using System.Web.Http;
using Service.Context;
using UI.Base;
using UI.Builders.Internship;
using UI.Builders.Internship.Data;
using UI.Builders.Master;
using UI.Events;

namespace Web.Api
{
    public class InternshipController : BaseApiController
    {
        readonly InternshipBuilder _internshipBuilder;

        public InternshipController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, InternshipBuilder internshipBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _internshipBuilder = internshipBuilder;
        }

        #region Actions

        [HttpPost]
        public async Task<IHttpActionResult> DeleteInternship(InternshipDeleteInternshipData query)
        {
            try
            {
                await _internshipBuilder.DeleteInternshipAsync(query.InternshipID);

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