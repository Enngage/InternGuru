using Core.Context;
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

namespace UI.Builders.Company
{
    public class FormBuilder : BaseBuilder
    {

        #region Constructor

        public FormBuilder(IAppContext appContext, IServicesLoader servicesLoader) : base(appContext, servicesLoader) { }

        #endregion

        #region Actions

        public async Task<FormInternshipView> BuildInternshipViewAsync(int internshipID, FormInternshipForm form = null)
        {
            var defaultForm = new FormInternshipForm()
            {
                InternshipID = internshipID,
                Message = null
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

        #endregion

        #region Public methods

        public async Task<int> SaveInternshipForm(FormInternshipForm form)
        {
            try
            {
                if (!this.CurrentUser.IsAuthenticated)
                {
                    // only authenticated users can send message
                    throw new UIException("Pro odeslání zprávy se prosím přihlašte");
                }

                // get internship
                var internship = await GetInternshipModelAsync(form.InternshipID);

                if (internship == null)
                {
                    throw new UIException($"Stáž s ID {form.InternshipID} nebyla nalezena");
                }

                // get recipient (company's representative)
                var companyUserID = await GetIDOfCompanyUserAsync(form.InternshipID);

                if (string.IsNullOrEmpty(companyUserID))
                {
                    throw new UIException($"Stáž u firmy {internship.CompanyName} nemá přiřazeného správce");
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
        /// Gets internship model from DB or Cache
        /// </summary>
        /// <param name="internshipID">Internship ID</param>
        /// <returns>Internship model</returns>
        private async Task<FormInternshipModel> GetInternshipModelAsync(int internshipID)
        {
            int cacheMinutes = 30;
            var cacheSetup = CacheService.GetSetup<FormInternshipModel>(this.GetSource(), cacheMinutes);
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
                    CompanyName = m.Company.CompanyName,
                    InternshipID = m.ID,
                    InternshipTitle = m.Title
                });

            return await CacheService.GetOrSetAsync(async () => await internshipQuery.FirstOrDefaultAsync(), cacheSetup);
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
