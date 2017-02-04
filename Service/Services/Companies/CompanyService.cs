using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Companies
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

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

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, company);
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

            return await SaveChangesAsync(SaveEventType.Insert, obj);
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

            // update company
            AppContext.Entry(company).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(company);

            // save changes
            return await SaveChangesAsync(SaveEventType.Update, obj, company);
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
