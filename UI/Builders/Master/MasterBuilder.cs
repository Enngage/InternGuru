using UI.Abstract;
using Core.Context;
using Cache;
using UI.Builders.Master.Models;
using Core.Services;

namespace UI.Builders.Master
{
    public class MasterBuilder : BuilderAbstract
    {

        #region Services

        #endregion

        #region Constructor

        public MasterBuilder(
            IAppContext appContext,
            ICacheService cacheService,
            IIdentityService identityService,
            ILogService logService,
            ICompanyService companyService
            ) : base(
                appContext,
                cacheService,
                identityService,
                logService,
                companyService)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes master model used by all views
        /// </summary>
        /// <returns>Master model</returns>
        public MasterModel GetMasterModel()
        {
            return new MasterModel()
            {
                CurrentUser = this.CurrentUser,
                CurrentCompany = this.CurrentCompany
            };
        }

        #endregion

    }
}
