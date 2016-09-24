

using Cache;
using Core.Services;
using UI.Files;

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

        public ServicesLoader(
                ICacheService cacheService,
                IInternshipService internshipService,
                ICompanyCategoryService companyCategoryService,
                IInternshipCategoryService internshipCategoryService,
                IFileProvider fileProvider,
                ICompanyService companyService,
                IIdentityService identityService,
                ILogService logService,
                IMessageService messageService
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
        }
    }
}
