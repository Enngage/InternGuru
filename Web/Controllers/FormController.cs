using Service.Context;
using System.Threading.Tasks;
using System.Web.Mvc;
using UI.Base;
using UI.Builders.Form;
using UI.Builders.Form.Forms;
using UI.Builders.Master;
using UI.Builders.Questionnaire;
using UI.Events;
using UI.Exceptions;

namespace Web.Controllers
{
    [Authorize]
    public class FormController : BaseController
    {
        readonly FormBuilder _formBuilder;
        private readonly QuestionnaireBuilder _questionnaireBuilder;

        public FormController(IAppContext appContext, IServiceEvents serviceEvents, MasterBuilder masterBuilder, FormBuilder formBuilder, QuestionnaireBuilder questionnaireBuilder) : base(appContext, serviceEvents, masterBuilder)
        {
            _formBuilder = formBuilder;
            _questionnaireBuilder = questionnaireBuilder;
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

            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    var model = await _formBuilder.BuildInternshipViewAsync(form.InternshipID);

                    if (model == null)
                    {
                        return HttpNotFound();
                    }

                    if (model.Internship.QuestionnaireID == null)
                    {
                        return HttpNotFound();
                    }

                    // get submitted questions
                    await _questionnaireBuilder.SubmitQuestionnaireFormAsync((int) model.Internship.QuestionnaireID, form.FieldGuids, Request);

                    await _formBuilder.SaveInternshipForm(form);

                    // set form status
                    model.InternshipForm.FormResult.IsSuccess = true;

                    // commit transaction
                    transaction.Commit();

                    return View(model);
                }
                catch (UiException ex)
                {
                    transaction.Rollback();

                    ModelStateWrapper.AddError(ex.Message);

                    var model = await _formBuilder.BuildInternshipViewAsync(form.InternshipID, form);

                    return View(model);
                }
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

            using (var transaction = AppContext.BeginTransaction())
            {
                try
                {
                    var model = await _formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                    // get submitted questions
                    await _questionnaireBuilder.SubmitQuestionnaireFormAsync((int)model.Thesis.QuestionnaireID, form.FieldGuids, Request);

                    await _formBuilder.SaveThesisForm(form);

                    // set form status
                    model.ThesisForm.FormResult.IsSuccess = true;

                    // commit transaction
                    transaction.Commit();

                    return View(model);
                }
                catch (UiException ex)
                {
                    transaction.Rollback();

                    ModelStateWrapper.AddError(ex.Message);

                    var model = await _formBuilder.BuildThesisViewAsync(form.ThesisID, form);

                    return View(model);
                }
            }
        }

        #endregion

        #region Helper methods



        #endregion
    }
}