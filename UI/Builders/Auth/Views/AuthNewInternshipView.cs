using UI.Builders.Auth.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthNewInternshipView : MasterView
    {
        public AuthAddEditInternshipForm InternshipForm { get; set; }

        /// <summary>
        /// Users can create internship only if they created company
        /// </summary>
        public bool CanCreateInternship { get; set; }
    }
}
