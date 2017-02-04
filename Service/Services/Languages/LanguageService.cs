using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Service.Exceptions;

namespace Service.Services.Languages
{
    public class LanguageService :  BaseService<Language>, ILanguageService
    {
        public static readonly char InternshipLanguageStringSeparator = ',';

        public LanguageService(IServiceDependencies serviceDependencies) : base(serviceDependencies) { }

        public override Task<int> InsertAsync(Language obj)
        {
            // set code name
            obj.CodeName = obj.GetCodeName();

            return base.InsertAsync(obj);
        }

        public override Task<int> UpdateAsync(Language obj)
        {
            var language = AppContext.Languages.Find(obj.ID);

            if (language == null)
            {
                throw new NotFoundException($"Languages with ID: {obj.ID} not found");
            }

            // set code name
            obj.CodeName = obj.GetCodeName();

            // save changes
            return base.UpdateAsync(obj, language);
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

        public override IDbSet<Language> GetEntitySet()
        {
            return this.AppContext.Languages;
        }
    }
}
