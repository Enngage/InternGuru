
using Common.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entity
{
    public class Company : EntityAbstract
    {

        public int ID { get; set; }
        [MaxLength(150)]
        public string CompanyName { get; set; }
        [Index]
        [MaxLength(100)]
        public string CodeName { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string LongDescription { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [MaxLength(100)]
        public string Country { get; set; }
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
        public int CompanySize { get; set; }
        [ForeignKey("CompanyCategory")]
        public int CompanyCategoryID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }

        #region Virtual properties

        public ApplicationUser ApplicationUser { get; set; }
        public CompanyCategory CompanyCategory { get; set; }
        public virtual ICollection<Internship> Internships { get; set; }

        #endregion

        #region EntityAbstract members

        public override object GetObjectID()
        {
            return ID;
        }
        public override string GetCodeName()
        {
            return CodeName;
        }

        #endregion

        #region Image methods

        /// <summary>
        /// Gets file name of banner from CompanyName
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static string GetBannerFileName(int companyID)
        {
            return companyID.ToString();
        }

        /// <summary>
        /// Gets file name of logo from CompanyName
        /// </summary>
        /// <param name="companyID">companyID</param>
        /// <returns></returns>
        public static string GetLogoFileName(int companyID)
        {
            return companyID.ToString();
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
