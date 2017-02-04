﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Events;
using Service.Exceptions;

namespace Service.Services.Languages
{
    public class LanguageService :  BaseService<Language>, ILanguageService
    {
        public static readonly char InternshipLanguageStringSeparator = ',';

        public LanguageService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public Task<int> DeleteAsync(int id)
        {
            var language = AppContext.Languages.Find(id);

            if (language != null)
            {
                AppContext.Languages.Remove(language);

                // touch cache keys
                TouchDeleteKeys(language);

                // save changes
                return SaveChangesAsync(SaveEventType.Delete, language);
            }

            return Task.FromResult(0);
        }

        public Task<Language> GetAsync(int id)
        {
            return AppContext.Languages.FirstOrDefaultAsync(m => m.ID == id);
        }

        public IQueryable<Language> GetAll()
        {
            return AppContext.Languages;
        }

        public IQueryable<Language> GetSingle(int id)
        {
            return AppContext.Languages.Where(m => m.ID == id).Take(1);
        }

        public Task<int> InsertAsync(Language obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            AppContext.Languages.Add(obj);

            // touch cache keys
            TouchInsertKeys(obj);

            return SaveChangesAsync(SaveEventType.Insert, obj);
        }

        public Task<int> UpdateAsync(Language obj)
        {
            var language = AppContext.Languages.Find(obj.ID);

            if (language == null)
            {
                throw new NotFoundException($"Languages with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // update log
            AppContext.Entry(language).CurrentValues.SetValues(obj);

            // touch cache keys
            TouchUpdateKeys(language);

            // save changes
            return SaveChangesAsync(SaveEventType.Update, obj, language);
        }

        public async Task<IEnumerable<Language>> GetAllCachedAsync()
        {
            return await CacheService.GetOrSetAsync(async () => await GetAll().ToListAsync(), GetCacheAllCacheSetup());
        }

        public async Task<IEnumerable<Language>> GetLanguagesFromCommaSeparatedStringAsync(string languagesCodeString)
        {
            var allLanguages = await GetAllCachedAsync();
            var result = new List<Language>();

            if (string.IsNullOrEmpty(languagesCodeString))
            {
                return result;
            }

            foreach (var languageCodeName in languagesCodeString.Split(InternshipLanguageStringSeparator))
            {
                // ReSharper disable once PossibleMultipleEnumeration
                var existingLanguage = allLanguages.FirstOrDefault(m => m.CodeName.Equals(languageCodeName, StringComparison.OrdinalIgnoreCase));

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
