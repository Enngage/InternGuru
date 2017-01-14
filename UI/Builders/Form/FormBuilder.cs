using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Entity.Base;
using Service.Exceptions;
using Service.Services.Activities.Enums;
using Service.Services.Thesis.Enums;
using UI.Base;
using UI.Builders.Form.Forms;
using UI.Builders.Form.Models;
using UI.Builders.Form.Views;
using UI.Builders.Services;
using UI.Builders.Shared.Models;
using UI.Exceptions;

namespace UI.Builders.Form
{
    public class FormBuilder : BaseBuilder
    {

        #region Constructor

        public FormBuilder(ISystemContext systemContext, IServicesLoader servicesLoader) : base(systemContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<FormInternshipView> BuildInternshipViewAsync(int internshipID, FormInternshipForm form = null)
        {
            var defaultForm = new FormInternshipForm()
            {
                Message = null,
                InternshipID = internshipID
            };

            var internship = await GetInternshipModelAsync(internshipID);

            if (internship == null)
            {
                return null;
            }

            return new FormInternshipView()
            {
                Internship = internship,
                InternshipForm = form == null ? defaultForm : form
            };

        }

        public async Task<FormThesisView> BuildThesisViewAsync(int thesisID, FormThesisForm form = null)
        {
            var defaultForm = new FormThesisForm()
            {
                ThesisID = thesisID,
                Message = null
            };

            var thesis = await GetThesisModelAsync(thesisID);

            if (thesis == null)
            {
                return null;
            }
           
            return new FormThesisView()
            {
                Thesis = thesis,
                ThesisForm = form == null ? defaultForm : form
            };

        }

        #endregion

        #region Public methods

        public async Task<int> SaveThesisForm(FormThesisForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send message
                    throw new ValidationException("Pro odeslání zprávy se prosím přihlašte");
                }

                // get thesis
                var thesis = await GetThesisModelAsync(form.ThesisID);

                if (thesis == null)
                {
                    throw new ValidationException($"Závěrečná práce s ID {form.ThesisID} nebyla nalezena");
                }

                // get recipient (company's representative)
                var companyUserID = await GetIDOfCompanyUserAsync(thesis.CompanyID);

                if (string.IsNullOrEmpty(companyUserID))
                {
                    throw new ValidationException($"Záveřečná práce u firmy {thesis.CompanyName} nemá přiřazeného správce");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = CurrentUser.Id,
                    RecipientCompanyID = thesis.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = thesis.ThesisName,
                    IsRead = false,
                };

                var messageID =  await Services.MessageService.InsertAsync(message);

                // log activity if we got this far
                var activityCurrentUserId = this.CurrentUser.IsAuthenticated ? this.CurrentUser.Id : null;
                await this.Services.ActivityService.LogActivity(ActivityTypeEnum.FormSubmitThesis, thesis.CompanyID, activityCurrentUserId, thesis.ID);

                return messageID;
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        public async Task<int> SaveInternshipForm(FormInternshipForm form)
        {
            try
            {
                if (!CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send message
                    throw new ValidationException("Pro odeslání zprávy se prosím přihlašte");
                }

                // get internship
                var internship = await GetInternshipModelAsync(form.InternshipID);

                if (internship == null)
                {
                    throw new ValidationException($"Stáž s ID {form.InternshipID} nebyla nalezena");
                }

                // get recipient (company's representative)
                var companyUserID = await GetIDOfCompanyUserAsync(internship.CompanyID);

                if (string.IsNullOrEmpty(companyUserID))
                {
                    throw new ValidationException($"Stáž u firmy {internship.CompanyName} nemá přiřazeného správce");
                }

                var message = new Message()
                {
                    SenderApplicationUserId = CurrentUser.Id,
                    RecipientCompanyID = internship.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = internship.InternshipTitle,
                    IsRead = false,
                };

                var messageID = await Services.MessageService.InsertAsync(message);

                // log activity if we got this far
                var activityCurrentUserId = this.CurrentUser.IsAuthenticated ? this.CurrentUser.Id : null;
                await this.Services.ActivityService.LogActivity(ActivityTypeEnum.FormSubmitInternship, internship.CompanyID, activityCurrentUserId, internship.InternshipID);

                // return message id
                return messageID;
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UiException(UiExceptionEnum.SaveFailure, ex);
            }
        }

        #endregion

        #region Helper methods

        /// <summary>
        /// Gets thesis model from DB or Cache
        /// </summary>
        /// <param name="thesisID">ThesisID</param>
        /// <returns>Internship model</returns>
        private async Task<FormThesisModel> GetThesisModelAsync(int thesisID)
        {
            var cacheSetup = Services.CacheService.GetSetup<FormThesisModel>(GetSource());
            cacheSetup.ObjectID = thesisID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID),
            };

            var thesisQuery = Services.ThesisService.GetSingle(thesisID)
                .Select(m => new FormThesisModel()
                {
                    CompanyID = m.Company.ID,
                    ThesisCodeName = m.ThesisType.CodeName,
                    CompanyName = m.Company.CompanyName,
                    CompanyCodeName = m.Company.CodeName,
                    ID = m.ID,
                    CompanyGuid = m.Company.CompanyGuid,
                    ThesisName = m.ThesisName,
                    ThesisTypeName = m.ThesisType.Name
                });

            var thesis = await Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);

            if (thesis == null)
            {
                return null;
            }

            // thesis name
            thesis.ThesisTypeNameConverted = thesis.ThesisCodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", (await GetAllThesisTypesAsync())) : thesis.ThesisTypeName;

            return thesis;
        }

        /// <summary>
        /// Gets all thesis types except the "all" type
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await Services.ThesisTypeService.GetAllCachedAsync())
                .Where(m => !m.CodeName.Equals(ThesisTypeEnum.All.ToString(), StringComparison.OrdinalIgnoreCase))
                .Select(m => m.Name)
                .ToList();
        }

        /// <summary>
        /// Gets internship model from DB or Cache
        /// </summary>
        /// <param name="internshipID">Internship ID</param>
        /// <returns>Internship model</returns>
        private async Task<FormInternshipModel> GetInternshipModelAsync(int internshipID)
        {
            var cacheSetup = Services.CacheService.GetSetup<FormInternshipModel>(GetSource());
            cacheSetup.ObjectID = internshipID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Internship>(internshipID),
                EntityKeys.KeyDelete<Entity.Internship>(internshipID),
            };

            var internshipQuery = Services.InternshipService.GetSingle(internshipID)
                .Select(m => new FormInternshipModel()
                {
                    CompanyID = m.CompanyID,
                    CompanyGuid = m.Company.CompanyGuid,
                    CompanyName = m.Company.CompanyName,
                    InternshipID = m.ID,
                    InternshipTitle = m.Title,
                    CompanyCodeName = m.CodeName,
                });

            return await Services.CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets ID of user who created company
        /// </summary>
        /// <param name="companyID">CompanyID</param>
        /// <returns>ID of user who created company</returns>
        private async Task<string> GetIDOfCompanyUserAsync(int companyID)
        {
            return await Services.CompanyService.GetSingle(companyID)
                .Select(m => m.ApplicationUserId)
                .FirstOrDefaultAsync();
        }

        #endregion

    }
}
