﻿
using System;

namespace UI.Builders.Auth.Models
{
    public class AuthInternshipListingModel
    {
        public string Title { get; set; }
        public string CodeName { get; set; }
        public int ID { get; set; }
        public DateTime Created { get; set; }
        public bool IsActive { get; set; }
        public int CompanyID { get; set; }
        public string CreatedByApplicationUserId { get; set; }
    }
}
