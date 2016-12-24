using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

using Service.Context;
using Entity;
using Cache;
using Service.Exceptions;
using System.Collections.Generic;
using System;

namespace Service.Services
{
    public class LanguageService :  BaseService<Language>, ILanguageService
    {

        public LanguageService(IAppContext appContext, ICacheService cacheService, ILogService logService) : base(appContext, cacheService, logService) { }

        public Task<int> DeleteAsync(int id)
        {
            var language = this.AppContext.Languages.Find(id);

            if (language != null)
            {
                this.AppContext.Languages.Remove(language);

                // touch cache keys
                this.TouchDeleteKeys(language);

                // fire event
                this.OnDelete(language);

                // save changes
                return this.AppContext.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task<Language> GetAsync(int id)
        {
            return this.AppContext.Languages.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Language> GetAll()
        {
            return this.AppContext.Languages;
        }

        public IQueryable<Language> GetSingle(int id)
        {
            return this.AppContext.Languages.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Language obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            this.AppContext.Languages.Add(obj);

            // touch cache keys
            this.TouchInsertKeys(obj);

            // fire event
            this.OnInsert(obj);

            return this.SaveChangesAsync();
        }

        public Task<int> UpdateAsync(Language obj)
        {
            var language = this.AppContext.Languages.Find(obj.ID);

            if (language == null)
            {
                throw new NotFoundException(string.Format("Languages with ID: {0} not found", obj.ID));
            }

            // fire event
            this.OnUpdate(obj, language);

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            this.AppContext.Entry(language).CurrentValues.SetValues(obj);

            // touch cache keys
            this.TouchUpdateKeys(language);

            // save changes
            return this.AppContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Language>> GetAllCachedAsync()
        {
            return await this.CacheService.GetOrSetAsync(async () => await this.GetAll().ToListAsync(), this.GetCacheAllCacheSetup());
        }

        public async Task<IEnumerable<Language>> GetLanguagesFromCommaSeparatedStringAsync(string languagesCodeString)
        {
            var allLanguages = await GetAllCachedAsync();
            var result = new List<Language>();

            if (string.IsNullOrEmpty(languagesCodeString))
            {
                throw new ArgumentException("Invalid languages code string, a comma separated string with langauge code names was expected");
            }

            foreach (var languageCodeName in languagesCodeString.Split(','))
            {
                var existingLanguage = allLanguages.Where(m => m.CodeName.Equals(languageCodeName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (existingLanguage != null)
                {
                    // language exists and is valid, add it to result
                    result.Add(existingLanguage);
                }
            }

            return result;
        }
    }
}
