
using Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Core.Config;
using Entity.Base;

namespace Entity
{
    public class Company : IEntity, IEntityWithTimeStamp, IEntityWithGuid, IEntityWithUniqueCodeName
    {
        public int ID { get; set; }
        [MaxLength(100)]
        [Index]
        public string CompanyName { get; set; }
        [Index]
        [MaxLength(100)]
        public string CodeName { get; set; }
        public Guid Guid { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string LongDescription { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        [MaxLength(250)]
        public string Web { get; set; }
        [MaxLength(250)]
        public string Twitter { get; set; }
        [MaxLength(250)]
        public string LinkedIn { get; set; }
        [MaxLength(250)]
        public string Facebook { get; set; }
        public int CompanyCategoryID { get; set; }
        public string ApplicationUserId { get; set; }
        public int CountryID { get; set; }
        public int CompanySizeID { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        #region Virtual properties

        [ForeignKey("CompanySizeID")]
        public CompanySize CompanySize { get; set; }
        [ForeignKey("CountryID")]
        public Country Country { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("CompanyCategoryID")]
        public CompanyCategory CompanyCategory { get; set; }
        public ICollection<Internship> Internships { get; set; }
        public ICollection<Message> Messages { get; set; }
        [ForeignKey("CompanyID")] // foreign key from "Thesis" entity
        public ICollection<Thesis> Theses { get; set; }

        #endregion

        #region IEntity members

        public object GetObjectID()
        {
            return ID;
        }
        public string GetCodeName()
        {
            return StringHelper.GetCodeName(CompanyName);
        }

        #endregion

        #region File methods

        /// <summary>
        /// Gets file name of banner for company
        /// </summary>
        /// <returns></returns>
        public static string GetBannerFileName()
        {
            return FileConfig.BannerFileName;
        }

        /// <summary>
        /// Gets file name of logo
        /// </summary>
        /// <returns></returns>
        public static string GetLogoFileName()
        {
            return FileConfig.LogoFileName;
        }

        /// <summary>
        /// Gets file name of company gallery image.
        /// Note: Use random guid using Guid.NewGuid() when creating a new image
        /// </summary>
        /// <param name="guid">guid</param>
        /// <returns></returns>
        public static string GetCompanyGalleryFileName(Guid guid)
        {
            return guid.ToString();
        }

        /// <summary>
        /// Gets path to company gallery folder
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns></returns>
        public static string GetCompanyGalleryFolderPath(Guid companyGuid)
        {
            return GetCompanyBaseFolderPath(companyGuid) + "/" + FileConfig.CompanyGalleryFolderName;
        }

        /// <summary>
        /// Gets path to company gallery file
        /// </summary>
        /// <param name="fileName">fileName</param>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns></returns>
        public static string GetCompanyGalleryFilePath(Guid companyGuid, string fileName)
        {
            return GetCompanyGalleryFolderPath(companyGuid) + "/" + fileName;
        }


        /// <summary>
        /// Gets path to company folder
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns>Path to company base folder</returns>
        public static string GetCompanyBaseFolderPath(Guid companyGuid)
        {
            return FileConfig.CompanyBaseFolderPath + "/" + companyGuid.ToString();
        }

        /// <summary>
        /// Gets path to company logo folder
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns></returns>
        public static string GetCompanyLogoFolderPath(Guid companyGuid)
        {
            return GetCompanyBaseFolderPath(companyGuid) + "/" + FileConfig.LogoFolderName;
        }

        /// <summary>
        /// Gets path to company banner folder
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns></returns>
        public static string GetCompanyBannerFolderPath(Guid companyGuid)
        {
            return GetCompanyBaseFolderPath(companyGuid) + "/" + FileConfig.BannerFolderName;
        }

        #endregion
    
    }
}
