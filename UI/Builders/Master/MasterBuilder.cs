using UI.Base;
using Service.Context;
using UI.Builders.Master.Models;
using UI.Builders.Services;

namespace UI.Builders.Master
{
    public class MasterBuilder : BaseBuilder
    {

        #region Constructor

        public MasterBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) {}

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
                CurrentCompany = this.CurrentCompany,
                StatusBox = this.StatusBox,
                UIHeader = this.UIHeader
            };
        }

        #endregion

        

    }
}
