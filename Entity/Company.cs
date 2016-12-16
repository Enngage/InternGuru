
using Core.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System;

namespace Entity
{
    public class Company : IEntity
    {

        public int ID { get; set; }
        [MaxLength(100)]
        [Index]
        public string CompanyName { get; set; }
        [Index]
        [MaxLength(100)]
        public string CodeName { get; set; }
        public Guid CompanyGUID { get; set; }
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
            return CodeName;
        }

        #endregion

        #region Image methods

        /// <summary>
        /// Gets file name of banner for company
        /// </summary>
        /// <param name="companyGuid"></param>
        /// <returns></returns>
        public static string GetBannerFileName(Guid companyGuid)
        {
            return companyGuid.ToString();
        }

        /// <summary>
        /// Gets file name of logo
        /// </summary>
        /// <param name="companyGuid">companyGuid</param>
        /// <returns></returns>
        public static string GetLogoFileName(Guid companyGuid)
        {
            return companyGuid.ToString();
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
        /// <param name="">Path -> FileConfig.cs</param>
        /// <returns></returns>
        public static string GetCompanyGalleryFolderPath(string path, Guid companyGuid)
        {
            return path + "/" + companyGuid.ToString();
        }

        #endregion

        #region Alias

        /// <summary>
        /// Gets alias based on Company name 
        /// </summary>
        /// <returns></returns>
        public string GetAlias()
        {
            return StringHelper.GetCodeName(CompanyName);
        }

        #endregion
    }
}
