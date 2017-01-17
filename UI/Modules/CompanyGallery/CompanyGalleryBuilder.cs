using System;
using System.IO;
using UI.Base;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;
using UI.Helpers;

namespace UI.Modules.CompanyGallery
{
    public class CompanyGalleryBuilder : BaseBuilder
    {

        #region Constructor

        public CompanyGalleryBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Web API methods

        public void DeleteImage(Guid companyGuid, string fileName)
        {
            // check if current user can delete image
            if (UserCanManageImages(companyGuid))
            {
                // check if file exists
                var relativePath = Entity.Company.GetCompanyGalleryFilePath(companyGuid, fileName);
                var systemFilePath = ImageHelper.GetSystemPathToFileStatic(relativePath);

                if (File.Exists(systemFilePath))
                {
                    File.Delete(systemFilePath);
                }
            }
            else
            {
                throw new UiException(UiExceptionEnum.NoPermission);
            }
        }

        #endregion

        #region Helper methods

        public bool UserCanManageImages(Guid companyGuid)
        {
            return CurrentCompany.CompanyGuid == companyGuid;
        }

        #endregion

    }
}
