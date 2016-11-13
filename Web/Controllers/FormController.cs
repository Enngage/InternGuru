using Core.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Company;
using UI.Builders.Form.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        FormBuilder formBuilder;

        public FormController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, FormBuilder formBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            this.formBuilder = formBuilder;
        }

        #region Actions

        public async Task<ActionResult> Internship(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await formBuilder.BuildInternshipViewAsync(id ?? 0);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        public async Task<ActionResult> Thesis(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await formBuilder.BuildThesisViewAsync(id ?? 0);

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Thesis(FormThesisForm form)
        {
            // validate form
            if (!this.ModelStateWrapper.IsValid)
            {
                var model = await formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                return View(model);
            }

            try
            {
                await formBuilder.SaveThesisForm(form);

                var model = await formBuilder.BuildThesisViewAsync(form.ThesisID, null);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UIException ex)
            {
                this.ModelStateWrapper.AddError(ex.Message);

                var model = await formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                return View(model);
            }
        }

        #endregion
    }
}