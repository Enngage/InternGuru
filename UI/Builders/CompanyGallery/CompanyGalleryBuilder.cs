using UI.Base;
using UI.Builders.Shared.Models;
using UI.Builders.Services;
using System;
using UI.Exceptions;
using UI.Helpers;
using Core.Config;
using System.IO;

namespace UI.Builders.Company
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
                var systemFilePath = ImageHelper.GetSystemPathToFile(relativePath);

                if (File.Exists(systemFilePath))
                {
                    File.Delete(systemFilePath);
                }
            }
            else
            {
                throw new UIException(UIExceptionEnum.NoPermission);
            }
        }

        #endregion

        #region Helper methods

        public bool UserCanManageImages(Guid companyGuid)
        {
            if (this.CurrentCompany.CompanyGUID == companyGuid)
            {
                return true;
            }
            return false;
        }

        #endregion

    }
}
