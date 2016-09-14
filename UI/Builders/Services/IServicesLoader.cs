using Cache;
using Core.Services;
using UI.Files;

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

        #endregion
    }
}
