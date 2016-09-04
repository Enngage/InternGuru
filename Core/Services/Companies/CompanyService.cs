using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Core.Context;
using Entity;
using Cache;
using Core.Exceptions;

namespace Core.Services
{
    public class CompanyService : ServiceAbstract, IService<Company>, ICompanyService
    {

        public CompanyService(IAppContext appContext, ICacheService cacheService) : base(appContext, cacheService) { }

        public Task DeleteAsync(int id)
        {
            // get log
            var company = this.AppContext.Companies.Find(id);

            if (company != null)
            {
                // delete company
                this.AppContext.Companies.Remove(company);

                // touch cache keys
                this.TouchDeleteKeys(company, company.ID.ToString());

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

        public Task InsertAsync(Company obj)
        {
            this.AppContext.Companies.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            return this.SaveChangesAsync();
        }

        public Task UpdateAsync(Company obj)
        {
            // get log
            var company = this.AppContext.Companies.Find(obj.ID);

            if (company == null)
            {
                throw new NotFoundException(string.Format("Company with ID: {0} not found", company.ID));
            }

            // update log
            this.AppContext.Entry(obj).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(company, company.ID.ToString());

            // save changes
            return this.AppContext.SaveChangesAsync();
        }
    }
}
