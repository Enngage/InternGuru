using Cache;
using EmailProvider;
using Service.Services.Activities;
using Service.Services.Companies;
using Service.Services.Countries;
using Service.Services.Currencies;
using Service.Services.Emails;
using Service.Services.Identity;
using Service.Services.Internships;
using Service.Services.Languages;
using Service.Services.Logs;
using Service.Services.Messages;
using Service.Services.Thesis;
using UI.UIServices;

namespace UI.Builders.Services
{
    public class ServicesLoader : IServicesLoader
    {
        public ICacheService CacheService { get; }
        public IInternshipService InternshipService { get; }
        public ICompanyCategoryService CompanyCategoryService { get; }
        public IInternshipCategoryService InternshipCategoryService { get; }
        public IFileProvider FileProvider { get; }
        public ICompanyService CompanyService { get; }
        public IIdentityService IdentityService { get; }
        public ILogService LogService { get; }
        public IMessageService MessageService { get; }
        public IInternshipDurationTypeService InternshipDurationTypeService { get; }
        public IInternshipAmountTypeService InternshipAmountTypeService { get; }
        public ICountryService CountryService { get; }
        public ICurrencyService CurrencyService { get; }
        public ICompanySizeService CompanySizeService { get; }
        public IThesisTypeService ThesisTypeService { get; }
        public IThesisService ThesisService { get; }
        public IEmailTemplateService EmailTemplateService { get; }
        public ICookieService CookieService { get; }
        public ILanguageService LanguageService { get; }
        public IHomeOfficeOptionService HomeOfficeOptionService { get; }
        public IStudentStatusOptionService StudentStatusOptionService { get; }
        public IActivityService ActivityService { get; }
        public IEmailService EmailService { get; }

        public ServicesLoader(
                ICacheService cacheService,
                IInternshipService internshipService,
                ICompanyCategoryService companyCategoryService,
                IInternshipCategoryService internshipCategoryService,
                IFileProvider fileProvider,
                ICompanyService companyService,
                IIdentityService identityService,
                ILogService logService,
                IMessageService messageService,
                IInternshipDurationTypeService internshipDurationService,
                IInternshipAmountTypeService internshipAmountTypeService,
                ICountryService countryService,
                ICurrencyService currencyService,
                ICompanySizeService companySizeService,
                IThesisService thesisService,
                IThesisTypeService thesisTypeService,
                IEmailTemplateService emailTemplateService,
                ICookieService cookieService,
                ILanguageService languageService,
                IHomeOfficeOptionService homeOfficeOptionService,
                IStudentStatusOptionService studentStatusOptionService,
                IActivityService activityService,
                IEmailService emailService
        )
        {
            CacheService = cacheService;
            InternshipService = internshipService;
            CompanyCategoryService = companyCategoryService;
            InternshipCategoryService = internshipCategoryService;
            FileProvider = fileProvider;
            CompanyService = companyService;
            IdentityService = identityService;
            LogService = logService;
            MessageService = messageService;
            InternshipDurationTypeService = internshipDurationService;
            InternshipAmountTypeService = internshipAmountTypeService;
            CountryService = countryService;
            CurrencyService = currencyService;
            CompanySizeService = companySizeService;
            ThesisService = thesisService;
            ThesisTypeService = thesisTypeService;
            EmailTemplateService = emailTemplateService;
            CookieService = cookieService;
            LanguageService = languageService;
            HomeOfficeOptionService = homeOfficeOptionService;
            StudentStatusOptionService = studentStatusOptionService;
            ActivityService = activityService;
            EmailService = emailService;
        }
    }
}
