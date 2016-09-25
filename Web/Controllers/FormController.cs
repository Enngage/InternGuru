using Common.Helpers;
using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Form.Forms;
using UI.Builders.Form.Views;
using UI.Builders.Master;
using UI.Exceptions;

namespace Web.Controllers
{
    public class FormController : BaseController
    {
        FormBuilder formBuilder;

        public FormController(IAppContext appContext, MasterBuilder masterBuilder, FormBuilder formBuilder) : base(appContext, masterBuilder)
        {
            this.formBuilder = formBuilder;
        }

        #region Actions

        public async Task<ActionResult> Internship(int id)
        {
            var model = await formBuilder.BuildInternshipViewAsync(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        #endregion

        #region POST actions

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Internship(FormInternshipForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                var model = await formBuilder.BuildInternshipViewAsync(form.InternshipID, form);

                return View(model);
            }

            try
            {
                await formBuilder.SaveInternshipForm(form);

                var model = await formBuilder.BuildInternshipViewAsync(form.InternshipID, null);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                var model = await formBuilder.BuildInternshipViewAsync(form.InternshipID, form);

                return View(model);
            }
        }

        #endregion
    }
}