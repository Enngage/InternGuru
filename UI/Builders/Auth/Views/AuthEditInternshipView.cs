using UI.Builders.Auth.Forms;

namespace UI.Builders.Auth.Views
{
    public class AuthEditInternshipView : AuthMasterView
    {
        public AuthAddEditInternshipForm InternshipForm { get; set; }

        /// <summary>
        /// Indicates if internship exists
        /// </summary>
        public bool InternshipExists
        {
            get
            {
                return InternshipForm != null;
            }
        }
    }
}
