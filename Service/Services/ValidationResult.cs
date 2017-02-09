namespace Service.Services
{
    public class ValidationResult : IValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }
}
