﻿
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Company : EntityAbstract
    {
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int YearFounded { get; set; }
        public string PublicEmail { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Address { get; set; }
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

        #region Virtual properties

        public CompanyCategory CompanyCategory { get; set; }

        #endregion
    }
}
