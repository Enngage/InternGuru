using Cache;
using Service.Services;
using EmailProvider;
using UI.Events;
using UI.UIServices;

namespace UI.Builders.Services
{
    /// <summary>
    /// Interface used to initialize all services for builders
    /// </summary>
    public interface IServicesLoader
    {
        #region Services

        ICacheService CacheService { get; }
        IInternshipService InternshipService { get; }
        ICompanyCategoryService CompanyCategoryService { get; }
        IInternshipCategoryService InternshipCategoryService { get; }
        IFileProvider FileProvider { get; }
        ICompanyService CompanyService { get; }
        IIdentityService IdentityService { get; }
        ILogService LogService { get; }
        IMessageService MessageService { get; }
        IInternshipDurationTypeService InternshipDurationTypeService { get; }
        IInternshipAmountTypeService InternshipAmountTypeService { get; }
        ICountryService CountryService { get; }
        ICurrencyService CurrencyService { get; }
        ICompanySizeService CompanySizeService { get; }
        IThesisService ThesisService { get; }
        IThesisTypeService ThesisTypeService { get; }
        IEmailTemplateService EmailTemplateService { get; }
        IEmailProvider EmailProvider { get; }
        ICookieService CookieService { get; }
        ILanguageService LanguageService { get; }
        IHomeOfficeOptionService HomeOfficeOptionService { get; }
        IStudentStatusOptionService StudentStatusOptionService { get; }

        #endregion
    }
}
