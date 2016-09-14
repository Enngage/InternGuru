
namespace UI.Builders.Shared
{
    /// <summary>
    /// Represent company of current user
    /// </summary>
    public interface ICurrentCompany
    {
        /// <summary>
        /// Company name
        /// </summary>
        string CompanyName { get; }

        /// <summary>
        /// Indicates if current user created company
        /// </summary>
        bool IsCreated { get; }

        /// <summary>
        /// ID of company
        /// </summary>
        int CompanyID { get; }
    }
}
