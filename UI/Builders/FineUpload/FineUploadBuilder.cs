using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Service.Exceptions;
using UI.Base;
using UI.Builders.Email.Views;
using UI.Builders.FineUpload.Models;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.FineUpload
{
    public  class FineUploadBuilder : BaseBuilder
    {

        #region Constructor

        public FineUploadBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region POST methods

        public void UploadCompanyGalleryFile(FineUploadModel upload)
        {
            try
            {
                if (!CurrentCompany.IsAvailable)
                {
                    throw new ValidationException($"Firma není dostupná");
                }

                if (upload == null)
                {
                    throw new ValidationException("Chybný soubor");
                }

                var galleryPath = Entity.Company.GetCompanyGalleryFolderPath(CurrentCompany.CompanyGuid);
                var fileNameToSave = Entity.Company.GetCompanyGalleryFileName(Guid.NewGuid()); // generate new guid for new images

                Services.FileProvider.SaveImage(upload.File, galleryPath, fileNameToSave);
            }
            catch (Exception ex)
            {
                Services.LogService.LogException(ex);

                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        #endregion
    }
}
