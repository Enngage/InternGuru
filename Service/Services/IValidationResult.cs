
namespace Service.Services
{
    public interface IValidationResult
    {
        /// <summary>
        /// Indicates if validated object is valid
        /// </summary>
        bool IsValid { get; set; }

        /// <summary>
        /// Error message in case object is not valid
        /// </summary>
        string ErrorMessage { get; set; }
    }
}
