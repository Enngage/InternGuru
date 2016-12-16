using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Service.Context;
using Entity;
using Cache;
using Service.Exceptions;
using System.Collections.Generic;

namespace Service.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            // get log
            var company = this.AppContext.Companies.Find(id);

            if (company != null)
            {
                // delete company
                this.AppContext.Companies.Remove(company);

                // touch cache keys
                this.TouchDeleteKeys(company);

                // fire event
                this.OnDelete(company);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Company> GetAsync(int id)
        {
            return this.AppContext.Companies.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Company> GetSingle(int id)
        {
            return this.AppContext.Companies.Where(m => m.ID == id).Take(1);
        }

        public IQueryable<Company> GetAll()
        {
            return this.AppContext.Companies;
        }

        public async Task<int> InsertAsync(Company obj)
        {
            // set company alias
            obj.CodeName = obj.GetAlias();

            // set company GUID
            obj.CompanyGUID = Guid.NewGuid();

            // check if alias is unique
            if (!await CompanyAliasIsUnique(obj.CodeName, 0))
            {
                throw new CodeNameNotUniqueException(string.Format("Company name {0} is not unique", obj.CodeName));
            }

            this.AppContext.Companies.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return await this.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(Company obj)
        {
            var company = this.AppContext.Companies.Find(obj.ID);

            if (company == null)
            {
                throw new NotFoundException(string.Format("Company with ID: {0} not found", company.ID));
            }

            // set company alias
            obj.CodeName = obj.GetAlias();

            // set company guid
            obj.CompanyGUID = company.CompanyGUID;

            // check if alias is unique
            if (!await CompanyAliasIsUnique(obj.CodeName, obj.ID))
            {
                throw new CodeNameNotUniqueException(string.Format("Company name {0} is not unique", obj.CodeName));
            }

            // fire event
            this.OnUpdate(obj, company);

            // update company
            this.AppContext.Entry(company).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(company);

            // save changes
            return await this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Company>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
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
            var companyQuery = this.GetAll()
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
