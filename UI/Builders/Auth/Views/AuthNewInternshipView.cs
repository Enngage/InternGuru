using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthNewInternshipView : AuthMasterView
    {
        public AuthAddEditInternshipForm InternshipForm { get; set; }

        /// <summary>
        /// Users can create internship only if they created company
        /// </summary>
        public bool CanCreateInternship { get; set; }
    }
}
