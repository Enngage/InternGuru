using UI.Builders.Auth.Forms;
using UI.Builders.Master.Views;

namespace UI.Builders.Auth.Views
{
    public class AuthEditInternshipView : MasterView
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
