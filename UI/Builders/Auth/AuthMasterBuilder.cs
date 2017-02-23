using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Config;
using Entity;
using Entity.Base;
using PagedList;
using Service.Extensions;
using UI.Base;
using UI.Builders.Auth.Models;
using UI.Builders.Auth.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Enums;
using UI.Builders.Shared.Models;

namespace UI.Builders.Auth
{
    public class AuthMasterBuilder : BaseBuilder
    {

        #region Setup

        private const string CandidateMasterView = "~/views/auth/CandidateTypeMaster.cshtml";
        private const string CompanyMasterview = "~/views/auth/CompanyTypeMaster.cshtml";

        #endregion

        #region Constructor

        public AuthMasterBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader)
        {
        }

        #endregion

        #region Master models

        public async Task<AuthMaster> GetAuthMasterModelAsync()
        {
            if (!CurrentUser.IsAuthenticated)
            {
                return null;
            }

            // NOTE: Inneficient - call only when user is created in future
            PrepareUserDirectories(CurrentUser.Id);

            var authMaster = new AuthMaster()
            {
                ShowUserTypeSelectionView = !CurrentUser.IsCandidate && !CurrentUser.IsCompany,
                AuthMasterLayout = CurrentUser.IsCompany ? CompanyMasterview : CandidateMasterView
            };

            if (CurrentUser.UserType == UserTypeEnum.Company)
            {
                authMaster.CompanyMaster = await GetCompanyMasterModelAsync();
            }
            if (CurrentUser.UserType == UserTypeEnum.Candidate)
            {
                authMaster.CandidateMaster = GetCandidateMasterModel();
            }

            return authMaster;
        }

        protected AuthCandidateMasterModel GetCandidateMasterModel()
        {
            return new AuthCandidateMasterModel();
        }

        protected async Task<AuthCompanyMasterModel> GetCompanyMasterModelAsync()
        {
            if (!CurrentUser.IsAuthenticated)
            {
                return null;
            }

                return new AuthCompanyMasterModel()
                {
                    Internships = await GetInternshipsAsync(),
                    Conversations = await GetConversationsAsync(10),
                    Theses = await GetThesesListingsAsync(),
                    Questionnaires = await GetQuestionnairesListingsAsync(),
                };
        }

        #endregion

        #region Actions

        public async Task<AuthCompanyTypeIndexView> BuildCompanyTypeIndexViewAsync(int? page)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthCompanyTypeIndexView()
            {
                AuthMaster = authMaster,
                ConversationsPaged = await GetConversationsAsync(page),
            };
        }

        public async Task<AuthCandidateTypeIndexView> BuildCandidateTypeIndexViewAsync(int? page)
        {
            var authMaster = await GetAuthMasterModelAsync();

            if (authMaster == null)
            {
                return null;
            }

            return new AuthCandidateTypeIndexView()
            {
                AuthMaster = authMaster,
            };
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Note: This method is inneficiently called each time an account page is accessed
        /// </summary>
        /// <param name="applicationUserId"></param>
        private void PrepareUserDirectories(string applicationUserId)
        {
            // create base folder
            var baseFolderPath = SystemConfig.ServerRootPath + "\\" + ApplicationUser.GetUserBaseFolderPath(applicationUserId);
            Directory.CreateDirectory(baseFolderPath);

            // create avatars folder
            var avatarsFolderPath = SystemConfig.ServerRootPath + "\\" + ApplicationUser.GetAvatarFolderPath(applicationUserId);
            Directory.CreateDirectory(avatarsFolderPath);
        }

        /// <summary>
        /// Gets messages of current user
        /// </summary>
        /// <param name="topN">Top N</param>
        /// <returns>Collection of messages of current user</returns>
        private async Task<IEnumerable<AuthConversationModel>> GetConversationsAsync(int topN)
        {
            return (await GetConversationsAsync(null)).Take(topN);
        }

        /// <summary>
        /// Gets messages of current user
        /// </summary>
        /// <param name="page">Page number</param>
        /// <returns>Collection of messages of current user</returns>
        private async Task<IPagedList<AuthConversationModel>> GetConversationsAsync(int? page)
        {
            var pageSize = 10;
            var pageNumber = (page ?? 1);

            // get both incoming and outgoming messages as well as messages targeted for given company
            var messagesQuery = Services.MessageService.GetAll()
                .Where(m =>
                    (m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApplicationUserId == CurrentUser.Id))
                .OrderByDescending(m => m.Created)
                .Select(m => new AuthMessageModel()
                {
                    ID = m.ID,
                    MessageCreated = m.Created,
                    SenderApllicationUserId = m.SenderApplicationUserId,
                    SenderApplicationUserName = m.SenderApplicationUser.UserName,
                    RecipientApplicationUserId = m.RecipientApplicationUserId,
                    RecipientApplicationUserName = m.RecipientApplicationUser.UserName,
                    MessageText = m.MessageText,
                    CurrentUserId = CurrentUser.Id,
                    IsRead = m.IsRead,
                    SenderFirstName = m.SenderApplicationUser.FirstName,
                    SenderLastName = m.SenderApplicationUser.LastName,
                    SenderNickname = m.SenderApplicationUser.Nickname,
                    RecipientNickname = m.RecipientApplicationUser.Nickname,
                    RecipientFirstName = m.RecipientApplicationUser.FirstName,
                    RecipientLastName = m.RecipientApplicationUser.LastName,
                    Subject = m.Subject,
                });

            var cacheSetupMessages = Services.CacheService.GetSetup<AuthMessageModel>(GetSource());
            cacheSetupMessages.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetupMessages.ObjectStringID = CurrentUser.Id;

            // get all messages for this user
            var allMessages = await Services.CacheService.GetOrSet(async () => await messagesQuery.ToListAsync(), cacheSetupMessages);

            // get conversations from messages
            var cacheSetupConversations = Services.CacheService.GetSetup<AuthConversationModel>(GetSource());
            cacheSetupConversations.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Message>(),
                EntityKeys.KeyDeleteAny<Message>(),
                EntityKeys.KeyCreateAny<Message>(),
            };
            cacheSetupConversations.ObjectStringID = CurrentUser.Id;

            var conversations = Services.CacheService.GetOrSet(() => FilterConversationMessages(allMessages), cacheSetupConversations);

            return conversations.ToPagedList(pageNumber, pageSize);
        }

        /// <summary>
        /// Creates list of conversations based on given messages
        /// </summary>
        /// <param name="messages">Messages</param>
        /// <returns>Conversation list</returns>
        private IEnumerable<AuthConversationModel> FilterConversationMessages(IEnumerable<AuthMessageModel> messages)
        {
            var conversationList = new List<AuthConversationModel>();

            foreach (var message in messages)
            {
                var existingConversationMessage = conversationList.Where(m => m.RecipientApplicationUserId == CurrentUser.Id || m.SenderApllicationUserId == CurrentUser.Id)
                    .FirstOrDefault();

                if (existingConversationMessage == null)
                {
                    // add message to conversations
                    conversationList.Add(new AuthConversationModel()
                    {
                        CurrentUserId = CurrentUser.Id,
                        ID = message.ID,
                        RecipientFirstName = message.RecipientFirstName,
                        RecipientLastName = message.RecipientLastName,
                        RecipientNickname = message.RecipientNickname,
                        SenderNickname = message.SenderNickname,
                        SenderFirstName = message.SenderFirstName,
                        SenderLastName = message.SenderLastName,
                        IsRead = message.IsRead,
                        MessageCreated = message.MessageCreated,
                        MessageText = message.MessageText,
                        RecipientApplicationUserId = message.RecipientApplicationUserId,
                        RecipientApplicationUserName = message.RecipientApplicationUserName,
                        SenderApllicationUserId = message.SenderApllicationUserId,
                        SenderApplicationUserName = message.SenderApplicationUserName,
                        Subject = message.Subject
                    });
                }
            }

            return conversationList;
        }

        /// <summary>
        /// Gets theses of current company/user
        /// </summary>
        /// <returns>Collection of theses</returns>
        private async Task<IEnumerable<AuthThesisListingModel>> GetThesesListingsAsync()
        {
            var thesisQuery = Services.ThesisService.GetAll()
                .ForUser(CurrentUser.Id)
                .Select(m => new AuthThesisListingModel()
                {
                    ID = m.ID,
                    IsActive = m.IsActive,
                    ThesisName = m.ThesisName,
                    Created = m.Created,
                    CodeName = m.CodeName,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId,
                    CompanyID = m.CompanyID
                });


            thesisQuery = thesisQuery.OrderByDescending(m => m.Created);

            var cacheSetup = Services.CacheService.GetSetup<AuthThesisListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Thesis>(),
                EntityKeys.KeyDeleteAny<Entity.Thesis>(),
                EntityKeys.KeyCreateAny<Entity.Thesis>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + CurrentCompany.CompanyID;

            return await Services.CacheService.GetOrSet(async () => await thesisQuery.ToListAsync(), cacheSetup);
        }

        private async Task<IEnumerable<AuthQuestionnaireListingModel>> GetQuestionnairesListingsAsync()
        {
            var questionnairesQuery = Services.QuestionnaireService.GetAll()
                .ForUser(CurrentUser.Id)
                .Select(m => new AuthQuestionnaireListingModel()
                {
                    ID = m.ID,
                    Updated = m.Updated,
                    QuestionnaireName = m.QuestionnaireName,
                    QuestionnaireXml = m.QuestionnaireXml,
                    CompanyID = m.CompanyID,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId
                });

            questionnairesQuery = questionnairesQuery.OrderByDescending(m => m.Updated);

            var cacheSetup = Services.CacheService.GetSetup<AuthQuestionnaireListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Questionnaire>(),
                EntityKeys.KeyDeleteAny<Entity.Questionnaire>(),
                EntityKeys.KeyCreateAny<Entity.Questionnaire>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id;

            var questionnaires = await Services.CacheService.GetOrSetAsync(async () => await questionnairesQuery.ToListAsync(), cacheSetup);

            // set questions 
            foreach (var questionnaire in questionnaires)
            {
                questionnaire.Questions = Services.QuestionnaireService.GetQuestionsFromXml(questionnaire.QuestionnaireXml).ToList();
            }

            return questionnaires;
        }


        /// <summary>
        /// Gets internships of current user
        /// </summary>
        /// <returns>Collection of internships of current user</returns>
        private async Task<IEnumerable<AuthInternshipListingModel>> GetInternshipsAsync()
        {
            var internshipsQuery = Services.InternshipService.GetAll()
                .ForUser(CurrentUser.Id)
                .Select(m => new AuthInternshipListingModel()
                {
                    ID = m.ID,
                    Title = m.Title,
                    Created = m.Created,
                    IsActive = m.IsActive,
                    CodeName = m.CodeName,
                    CompanyID = m.CompanyID,
                    CreatedByApplicationUserId = m.CreatedByApplicationUserId
                });

            internshipsQuery = internshipsQuery.OrderByDescending(m => m.Created);

            var cacheSetup = Services.CacheService.GetSetup<AuthInternshipListingModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id + "_" + CurrentCompany.CompanyID;

            return await Services.CacheService.GetOrSet(async () => await internshipsQuery.ToListAsync(), cacheSetup);
        }

        #endregion

        #region Form helper methods

        /// <summary>
        /// Gets company categories and caches the result
        /// </summary>
        /// <returns>Collection of company categories</returns>
        protected async Task<IEnumerable<AuthCompanyCategoryModel>> FormGetCompanyCategoriesAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<AuthCompanyCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<CompanyCategory>(),
                EntityKeys.KeyDeleteAny<CompanyCategory>(),
                EntityKeys.KeyUpdateAny<CompanyCategory>(),
            };

            var companyCategoriesQuery = Services.CompanyCategoryService.GetAll()
                .Select(m => new AuthCompanyCategoryModel()
                {
                    CompanyCategoryID = m.ID,
                    CompanyCategoryName = m.Name
                });

            var companyCategories = await Services.CacheService.GetOrSetAsync(async () => await companyCategoriesQuery.ToListAsync(), cacheSetup);

            return companyCategories;
        }

        /// <summary>
        /// Gets internship categories and caches the result
        /// </summary>
        /// <returns>Collection of internship categories</returns>
        protected async Task<IEnumerable<AuthInternshipCategoryModel>> FormGetInternshipCategoriesAsync()
        {
            var cacheSetup = Services.CacheService.GetSetup<AuthInternshipCategoryModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyCreateAny<InternshipCategory>(),
                EntityKeys.KeyDeleteAny<InternshipCategory>(),
                EntityKeys.KeyUpdateAny<InternshipCategory>(),
            };

            var internshipCategoriesQuery = Services.InternshipCategoryService.GetAll()
                .Select(m => new AuthInternshipCategoryModel()
                {
                    InternshipCategoryID = m.ID,
                    InternshipCategoryName = m.Name
                });

            var internshipCategories = await Services.CacheService.GetOrSetAsync(async () => await internshipCategoriesQuery.ToListAsync(), cacheSetup);

            return internshipCategories;
        }

        protected async Task<IEnumerable<AuthInternshipAmountType>> FormGetInternshipAmountTypesAsync()
        {
            var internshipAmountTypes = await Services.InternshipAmountTypeService.GetAllCachedAsync();

            return internshipAmountTypes.Select(m => new AuthInternshipAmountType()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                AmountTypeName = m.AmountTypeName
            });
        }

        protected async Task<IEnumerable<AuthInternshipDurationType>> FormGetInternshipDurationsAsync()
        {
            var durationTypes = await Services.InternshipDurationTypeService.GetAllCachedAsync();

            return durationTypes.Select(m => new AuthInternshipDurationType()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                DurationName = m.DurationName,
                DurationTypeEnum = m.DurationTypeEnum
            });
        }

        protected async Task<IEnumerable<AuthThesisTypeModel>> FormGetThesisTypesAsync()
        {
            var thesisTypes = await Services.ThesisTypeService.GetAllCachedAsync();

            return thesisTypes.Select(m => new AuthThesisTypeModel()
            {
                ID = m.ID,
                Name = m.Name
            });
        }

        protected async Task<IEnumerable<AuthCurrencyModel>> FormGetCurrenciesAsync()
        {
            var currencies = await Services.CurrencyService.GetAllCachedAsync();

            return currencies.Select(m => new AuthCurrencyModel()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CurrencyName = m.CurrencyName
            });
        }

        protected async Task<IEnumerable<AuthInternshipLanguageModel>> FormGetLanguagesAsync()
        {
            var languages = await Services.LanguageService.GetAllCachedAsync();

            return languages.Select(m => new AuthInternshipLanguageModel()
            {
                CodeName = m.CodeName,
                LanguageName = m.LanguageName
            });
        }

        protected async Task<IEnumerable<AuthCountryModel>> FormGetCountriesAsync()
        {
            var countries = await Services.CountryService.GetAllCachedAsync();

            return countries.Select(m => new AuthCountryModel()
            {
                ID = m.ID,
                CountryCode = m.CountryCode,
                CountryName = m.CountryName,
                Icon = m.Icon
            });
        }

        protected async Task<IEnumerable<AuthQuestionnaireModel>> FormGetQuestionnairesAsync()
        {
            var questionnairesQuery = Services.QuestionnaireService.GetAll()
                .ForUser(CurrentUser.Id)
                .Select(m => new AuthQuestionnaireModel()
                {
                    ID = m.ID,
                    QuestionnaireName = m.QuestionnaireName
                })
                .OrderByDescending(m => m.ID);


            var cacheSetup = Services.CacheService.GetSetup<AuthQuestionnaireModel>(GetSource());
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdateAny<Entity.Internship>(),
                EntityKeys.KeyDeleteAny<Entity.Internship>(),
                EntityKeys.KeyCreateAny<Entity.Internship>(),
            };
            cacheSetup.ObjectStringID = CurrentUser.Id;

            return await Services.CacheService.GetOrSet(async () => await questionnairesQuery.ToListAsync(), cacheSetup);
        }

        protected async Task<IEnumerable<AuthCompanySize>> FormGetCompanySizesAsync()
        {
            var companySizes = await Services.CompanySizeService.GetAllCachedAsync();

            return companySizes.Select(m => new AuthCompanySize()
            {
                ID = m.ID,
                CodeName = m.CodeName,
                CompanySizeName = m.CompanySizeName
            });
        }


        protected async Task<IEnumerable<AuthInternshipEducationTypeModel>> FormGetEducationTypesAsync()
        {
            return (await Services.EducationTypeService.GetAllCachedAsync())
                .Select(m => new AuthInternshipEducationTypeModel()
                {
                    CodeName = m.CodeName,
                    Name = m.Name,
                    ID = m.ID
                });
        }

        protected async Task<IEnumerable<AuthInternshipStudentStatusOptionModel>> FormGetAllStudentStatusOptionsAsync()
        {
            return (await Services.StudentStatusOptionService.GetAllCachedAsync())
                .Select(m => new AuthInternshipStudentStatusOptionModel()
                {
                    CodeName = m.CodeName,
                    StudentStatusName = m.StatusName,
                    ID = m.ID
                });
        }

        protected async Task<AuthInternshipDurationType> GetDurationTypeAsync(int durationID)
        {
            var durationTypes = await FormGetInternshipDurationsAsync();
            return durationTypes
                .Where(m => m.ID == durationID)
                .SingleOrDefault();
        }

        #endregion
    }
}
