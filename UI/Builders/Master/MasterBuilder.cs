using UI.Base;
using UI.Builders.Master.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;

namespace UI.Builders.Master
{
    public class MasterBuilder : BaseBuilder
    {

        #region Constructor

        public MasterBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

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
                CurrentUser = CurrentUser,
                CurrentCompany = CurrentCompany,
                StatusBox = StatusBox,
                UiHeader = UiHeader
            };
        }

        #endregion

    }
}
