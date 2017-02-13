using Service.Services;

namespace Service.Models
{
    public class ValidationResult : IValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
