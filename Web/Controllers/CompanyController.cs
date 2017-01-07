﻿using Core.Helpers;
using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Company.Enums;
using UI.Builders.Company.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    public class CompanyController : BaseController
    {
        readonly CompanyBuilder _companyBuilder;

        public CompanyController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, CompanyBuilder companyBuilder) : base (appContext, serviceEvents, masterBuilder)
        {
            _companyBuilder = companyBuilder;
        }

        #region Actions

        public async Task<ActionResult> Index(string codeName, string tab)
        {
            if (string.IsNullOrEmpty(codeName))
            {
                return HttpNotFound();
            }

            var model = await _companyBuilder.BuildDetailViewAsync(codeName);

            if (model == null)
            {
                return HttpNotFound();
            }

            // set tab if possible
            var activeTab = EnumHelper.ParseEnum(tab, CompanyDetailMenuEnum.About);
            model.ActiveTab = activeTab;

            return View(model);
        }

        #endregion

        #region POST actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(CompanyContactUsForm form)
        {
            // active tab (indicates which right menu is active)
            var activeTab = CompanyDetailMenuEnum.Contact;

            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);
                model.ActiveTab = activeTab;

                return View(model);
            }

            try
            {
                await _companyBuilder.CreateMessage(form);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName);

                // set active tab
                model.ActiveTab = activeTab;

                // set form status
                model.ContactUsForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                var model = await _companyBuilder.BuildDetailViewAsync(form.CompanyCodeName, form);
                model.ActiveTab = activeTab;

                return View(model);
            }
        }

        #endregion
    }
}