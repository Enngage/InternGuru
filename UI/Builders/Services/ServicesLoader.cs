using Cache;
using Core.Services;
using EmailProvider;
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
                IEmailProvider emailProvider
        )
        {
            this.CacheService = cacheService;
            this.InternshipService = internshipService;
            this.CompanyCategoryService = companyCategoryService;
            this.InternshipCategoryService = internshipCategoryService;
            this.FileProvider = fileProvider;
            this.CompanyService = companyService;
            this.IdentityService = identityService;
            this.LogService = logService;
            this.MessageService = messageService;
            this.InternshipDurationTypeService = internshipDurationService;
            this.InternshipAmountTypeService = internshipAmountTypeService;
            this.CountryService = countryService;
            this.CurrencyService = currencyService;
            this.CompanySizeService = companySizeService;
            this.ThesisService = thesisService;
            this.ThesisTypeService = thesisTypeService;
            this.EmailTemplateService = emailTemplateService;
            this.EmailProvider = emailProvider;
        }
    }
}
