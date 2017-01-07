using Cache;
using EmailProvider;
using Service.Services.Activities;
using Service.Services.Companies;
using Service.Services.Countries;
using Service.Services.Currencies;
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
        public ICacheService CacheService { get; private set; }
        public IInternshipService InternshipService { get; private set; }
        public ICompanyCategoryService CompanyCategoryService { get; private set; }
        public IInternshipCategoryService InternshipCategoryService { get; private set; }
        public IFileProvider FileProvider { get; private set; }
        public ICompanyService CompanyService { get; private set; }
        public IIdentityService IdentityService { get; private set; }
        public ILogService LogService { get; private set; }
        public IMessageService MessageService { get; private set; }
        public IInternshipDurationTypeService InternshipDurationTypeService { get; private set; }
        public IInternshipAmountTypeService InternshipAmountTypeService { get; private set; }
        public ICountryService CountryService { get; private set; }
        public ICurrencyService CurrencyService { get; private set; }
        public ICompanySizeService CompanySizeService { get; private set; }
        public IThesisTypeService ThesisTypeService { get; private set; }
        public IThesisService ThesisService { get; private set; }
        public IEmailTemplateService EmailTemplateService { get; private set; }
        public IEmailProvider EmailProvider { get; private set; }
        public ICookieService CookieService { get; private set; }
        public ILanguageService LanguageService { get; private set; }
        public IHomeOfficeOptionService HomeOfficeOptionService { get; private set; }
        public IStudentStatusOptionService StudentStatusOptionService { get; private set; }
        public IActivityService ActivityService { get; private set; }

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
                IEmailProvider emailProvider,
                ICookieService cookieService,
                ILanguageService languageService,
                IHomeOfficeOptionService homeOfficeOptionService,
                IStudentStatusOptionService studentStatusOptionService,
                IActivityService activityService
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
            EmailProvider = emailProvider;
            CookieService = cookieService;
            LanguageService = languageService;
            HomeOfficeOptionService = homeOfficeOptionService;
            StudentStatusOptionService = studentStatusOptionService;
            ActivityService = activityService;
        }
    }
}
