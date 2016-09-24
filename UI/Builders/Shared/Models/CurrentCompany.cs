
namespace UI.Builders.Shared
{
    public class CurrentCompany : ICurrentCompany
    {
        /// <summary>
        /// Company name
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Indicates if current user created company
        /// </summary>
        public bool IsCreated
        {
            get
            {
                return !string.IsNullOrEmpty(CompanyName);
            }
        }

        /// <summary>
        /// ID of company
        /// </summary>
        public int CompanyID { get; set; }


        /// <summary>
        /// ID of user who created company
        /// </summary>
        public string CompanyCreatedByApplicationUserId { get; set; }

        /// <summary>
        /// Name of user who created company
        /// </summary>
        public string CompanyCreatedByApplicationUserName { get; set; }
    }
}
