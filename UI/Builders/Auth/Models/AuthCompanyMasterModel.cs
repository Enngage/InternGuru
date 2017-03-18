using System.Collections.Generic;

namespace UI.Builders.Auth.Models
{
    public class AuthCompanyMasterModel
    {
        public IEnumerable<AuthInternshipListingModel> Internships { get; set; }
        public IEnumerable<AuthThesisListingModel> Theses { get; set; }
        public IEnumerable<AuthQuestionnaireListingModel> Questionnaires { get; set; }

        public int QuestionnaireSubmissionsCount { get; set; }

    }
}
