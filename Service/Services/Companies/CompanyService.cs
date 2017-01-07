using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Cache;
using Entity;
using Service.Context;
using Service.Exceptions;
using Service.Services.Logs;

namespace Service.Services.Companies
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            // get log
            var company = AppContext.Companies.Find(id);

            if (company != null)
            {
                // delete company
                AppContext.Companies.Remove(company);

                // touch cache keys
                TouchDeleteKeys(company);

                // fire event
                OnDelete(company);

                // save changes
                return AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Company> GetAsync(int id)
        {
            return AppContext.Companies.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Company> GetSingle(int id)
        {
            return AppContext.Companies.Where(m => m.ID == id).Take(1);
        }

        public IQueryable<Company> GetAll()
        {
            return AppContext.Companies;
        }

        public async Task<int> InsertAsync(Company obj)
        {
            // set company alias
            obj.CodeName = obj.GetAlias();

            // set company GUID
            obj.CompanyGuid = Guid.NewGuid();

            // check if alias is unique
            if (!await CompanyAliasIsUnique(obj.CodeName, 0))
            {
                throw new CodeNameNotUniqueException($"Company name {obj.CodeName} is not unique");
            }

            AppContext.Companies.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            // fire event
            OnInsert(obj);

            return await SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Company obj)
        {
            var company = AppContext.Companies.Find(obj.ID);

            if (company == null)
            {
                throw new NotFoundException($"Company with ID: {obj.ID} not found");
            }

            // set company alias
            obj.CodeName = obj.GetAlias();

            // set company guid
            obj.CompanyGuid = company.CompanyGuid;

            // check if alias is unique
            if (!await CompanyAliasIsUnique(obj.CodeName, obj.ID))
            {
                throw new CodeNameNotUniqueException($"Company name {obj.CodeName} is not unique");
            }

            // fire event
            OnUpdate(obj, company);

            // update company
            AppContext.Entry(company).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(company);

            // save changes
            return await AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

        #region Helper methods

        /// <summary>
        /// Indicates if given alias is unique
        /// </summary>
        /// <param name="alias">Alias to check for uniqueness</param>
        /// <param name="companyID">CompanyID of udpated company. Use 0 when creating new company</param>
        /// <returns>True if unique, false otherwise</returns>
        private async Task<bool> CompanyAliasIsUnique(string alias, int companyID)
        {
            var companyQuery = GetAll()
                .Where(m => m.CodeName == alias)
                .Take(1);

            if (companyID != 0)
            {
                companyQuery = companyQuery.Where(m => m.ID != companyID);
            }

            var company = await companyQuery
                .Select(m => m.ID)
                .FirstOrDefaultAsync();

            return company == 0;
        }

        #endregion
    }
}
