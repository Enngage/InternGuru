using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Form;
using UI.Builders.Form.Forms;
using UI.Builders.Master;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        readonly FormBuilder _formBuilder;

        public FormController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, FormBuilder formBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _formBuilder = formBuilder;
        }

        #region Actions

        [Route("Zajem/Staz/{id}/{codeName}")]
        public async Task<ActionResult> Internship(int? id, string codeName)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _formBuilder.BuildInternshipViewAsync((int) id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [Route("Zajem/ZaverecnaPrace/{id}/{codeName}")]
        public async Task<ActionResult> Thesis(int? id, string codeName)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = await _formBuilder.BuildThesisViewAsync((int) id);

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
        [Route("Zajem/Staz/{id}/{codeName}")]
        public async Task<ActionResult> Internship(FormInternshipForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                var model = await _formBuilder.BuildInternshipViewAsync(form.InternshipID, form);

                return View(model);
            }

            try
            {
                await _formBuilder.SaveInternshipForm(form);

                var model = await _formBuilder.BuildInternshipViewAsync(form.InternshipID);

                // set form status
                model.InternshipForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                var model = await _formBuilder.BuildInternshipViewAsync(form.InternshipID, form);

                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Zajem/ZaverecnaPrace/{id}/{codeName}")]
        public async Task<ActionResult> Thesis(FormThesisForm form)
        {
            // validate form
            if (!ModelStateWrapper.IsValid)
            {
                var model = await _formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                return View(model);
            }

            try
            {
                await _formBuilder.SaveThesisForm(form);

                var model = await _formBuilder.BuildThesisViewAsync(form.ThesisID);

                // set form status
                model.ThesisForm.FormResult.IsSuccess = true;

                return View(model);
            }
            catch (UiException ex)
            {
                ModelStateWrapper.AddError(ex.Message);

                var model = await _formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                return View(model);
            }
        }

        #endregion
    }
}