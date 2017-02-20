using PagedList;
using UI.Builders.Auth.Models;

namespace UI.Builders.Auth.Views
{
    public class AuthQuestionnaireSubmissionsView : AuthMasterView
    {
        public IPagedList<AuthQuestionnaireSubmissionModel> SubmissionsPaged { get; set; }
    }
}
