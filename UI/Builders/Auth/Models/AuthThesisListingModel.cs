using System;

namespace UI.Builders.Auth.Models
{
    public class AuthThesisListingModel
    {
        public string ThesisName { get; set; }
        public int ID { get; set; }
        public bool IsActive { get; set; }
        public DateTime Created { get; set; }
        public string CodeName { get; set; }
        public int CompanyID { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
