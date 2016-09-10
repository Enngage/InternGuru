
using Common.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Company : EntityAbstract
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string LongDescription { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string Web { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
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

        #region Image methods

        /// <summary>
        /// Gets file name of banner from CompanyName
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static string GetBannerFileName(string companyName)
        {
            return StringHelper.GetCodeName(companyName);
        }

        /// <summary>
        /// Gets file name of logo from CompanyName
        /// </summary>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static string GetLogoFileName(string companyName)
        {
            return StringHelper.GetCodeName(companyName);
        }

        #endregion
    }
}
