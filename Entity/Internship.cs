using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entity
{
    public class Internship : EntityAbstract
    {
        public int ID { get; set; }
        
        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        [ForeignKey("Company")]
        public int CompanyID { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string City { get; set; } 
        public string Country { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public bool IsPaid { get; set; }
        public int DurationInMonths { get; set; }
        public int DurationInWeeks { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        #region Virtual

        public Category Category { get; set; }
        public Company Company { get; set; }

        #endregion
    }
}