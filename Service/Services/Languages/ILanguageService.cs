﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace Service.Services.Languages
{
    public interface ILanguageService : IService<Language>
    {
        /// <summary>
        /// Gets list of Languages based on string (used eg. in Internship where languages are stored as a comma separated string)
        /// </summary>
        /// <param name="languagesCodeString"></param>
        /// <returns></returns>
        Task<IEnumerable<Language>> GetLanguagesFromCommaSeparatedStringAsync(string languagesCodeString);
    }
}