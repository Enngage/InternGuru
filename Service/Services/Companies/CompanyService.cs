using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Companies
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {

        public CompanyService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override async Task<int> InsertAsync(Company obj)
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

            return await base.InsertAsync(obj);
        }

        public override async Task<int> UpdateAsync(Company obj)
        {
            var company = EntityDbSet.Find(obj.ID);

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

            // save changes
            return await base.UpdateAsync(obj, company);
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

        public override IDbSet<Company> GetEntitySet()
        {
            return EntitySet;
        }

        public IDbSet<Company> EntitySet => this.AppContext.Companies;

        #endregion
    }
}
