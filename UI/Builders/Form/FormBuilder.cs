using Service.Context;
using System.Data.Entity;
using System.Linq;
using UI.Base;
using UI.Builders.Company.Views;
using System.Threading.Tasks;
using UI.Builders.Services;
using System;
using UI.Exceptions;
using Entity;
using UI.Builders.Form.Models;
using UI.Builders.Form.Views;
using System.Collections.Generic;
using UI.Builders.Form.Forms;
using Service.Services.Enums;
using Service.Exceptions;
using UI.Builders.Shared.Models;

namespace UI.Builders.Company
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
                if (!this.CurrentUser.IsAuthenticated)
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
                    SenderApplicationUserId = this.CurrentUser.Id,
                    RecipientCompanyID = thesis.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = thesis.ThesisName,
                    IsRead = false,
                };

                return await this.Services.MessageService.InsertAsync(message);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
            }
        }

        public async Task<int> SaveInternshipForm(FormInternshipForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
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
                    SenderApplicationUserId = this.CurrentUser.Id,
                    RecipientCompanyID = internship.CompanyID,
                    RecipientApplicationUserId = companyUserID,
                    MessageText = form.Message,
                    Subject = internship.InternshipTitle,
                    IsRead = false,
                };

                return await this.Services.MessageService.InsertAsync(message);
            }
            catch (ValidationException ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                // log error
                Services.LogService.LogException(ex);

                // re-throw
                throw new UIException(UIExceptionEnum.SaveFailure, ex);
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
            int cacheMinutes = 30;
            var cacheSetup = this.Services.CacheService.GetSetup<FormThesisModel>(this.GetSource(), cacheMinutes);
            cacheSetup.ObjectID = thesisID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Thesis>(thesisID),
                EntityKeys.KeyDelete<Entity.Thesis>(thesisID),
            };

            var thesisQuery = this.Services.ThesisService.GetSingle(thesisID)
                .Select(m => new FormThesisModel()
                {
                    CompanyID = m.Company.ID,
                    ThesisCodeName = m.ThesisType.CodeName,
                    CompanyName = m.Company.CompanyName,
                    ID = m.ID,
                    CompanyGuid = m.Company.CompanyGUID,
                    ThesisName = m.ThesisName,
                    ThesisTypeName = m.ThesisType.Name
                });

            var thesis = await this.Services.CacheService.GetOrSetAsync(async () => await thesisQuery.FirstOrDefaultAsync(), cacheSetup);

            if (thesis == null)
            {
                return null;
            }

            // thesis name
            thesis.ThesisTypeNameConverted = thesis.ThesisCodeName.Equals(ThesisTypeEnum.all.ToString(), StringComparison.OrdinalIgnoreCase) ? string.Join("/", (await GetAllThesisTypesAsync())) : thesis.ThesisTypeName;

            return thesis;
        }

        /// <summary>
        /// Gets all thesis types except the "all" type
        /// </summary>
        /// <returns></returns>
        private async Task<List<string>> GetAllThesisTypesAsync()
        {
            return (await this.Services.ThesisTypeService.GetAllCachedAsync())
                .Where(m => !m.CodeName.Equals(ThesisTypeEnum.all.ToString(), StringComparison.OrdinalIgnoreCase))
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
            int cacheMinutes = 30;
            var cacheSetup = this.Services.CacheService.GetSetup<FormInternshipModel>(this.GetSource(), cacheMinutes);
            cacheSetup.ObjectID = internshipID;
            cacheSetup.Dependencies = new List<string>()
            {
                EntityKeys.KeyUpdate<Entity.Internship>(internshipID),
                EntityKeys.KeyDelete<Entity.Internship>(internshipID),
            };

            var internshipQuery = this.Services.InternshipService.GetSingle(internshipID)
                .Select(m => new FormInternshipModel()
                {
                    CompanyID = m.CompanyID,
                    CompanyGuid = m.Company.CompanyGUID,
                    CompanyName = m.Company.CompanyName,
                    InternshipID = m.ID,
                    InternshipTitle = m.Title
                });

            return await this.Services.CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);
        }

        /// <summary>
        /// Gets ID of user who created company
        /// </summary>
        /// <param name="companyID">CompanyID</param>
        /// <returns>ID of user who created company</returns>
        private async Task<string> GetIDOfCompanyUserAsync(int companyID)
        {
            return await this.Services.CompanyService.GetSingle(companyID)
                .Select(m => m.ApplicationUserId)
                .FirstOrDefaultAsync();
        }

        #endregion

    }
}
